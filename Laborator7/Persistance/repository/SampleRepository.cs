using Domain.domain;
using log4net;
using Persistance.connectionUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.repository
{

    public class SampleRepository : ISampleRepository
    {

        public static readonly ILog logger = LogManager.GetLogger("SampleRepository");

        public SampleRepository()
        {
            logger.Info("Creating the Office Repository");
        }


        public Sample findOne(string id)
        {
            logger.InfoFormat("Entering findOne with value {0}", id);
            IDbConnection con = DBUtils.getConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Sample where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        //string idProba = dataR.GetString(0);
                        String distantaProbaS = dataR.GetString(1);
                        String stilProbaS = dataR.GetString(2);

                        Sample proba = new Sample((Distance)Enum.Parse(typeof(Distance), distantaProbaS), (Style)Enum.Parse(typeof(Style), stilProbaS))
                        {
                            Id = id
                        };

                        logger.InfoFormat("Exiting findOne with value {0}", proba);
                        return proba;

                    }
                }
            }
            logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }

        public List<Sample> findAll()
        {
            IDbConnection con = DBUtils.getConnection();
            List<Sample> probaR = new List<Sample>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Sample";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string idProba = dataR.GetString(0);
                        String distantaProbaS = dataR.GetString(1);
                        String stilProbaS = dataR.GetString(2);
                        Sample proba = new Sample((Distance)Enum.Parse(typeof(Distance), distantaProbaS), (Style)Enum.Parse(typeof(Style), stilProbaS))
                        {
                            Id = idProba
                        };

                        probaR.Add(proba);
                    }
                }
            }
            return probaR;
        }

        public void save(Sample entity)
        {
            logger.InfoFormat("Se salveaza cursa cu id-il {0}", entity.Id);
            var con = DBUtils.getConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Sample values (@id, @distance, @style)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                var paramDistanta = comm.CreateParameter();
                paramDistanta.ParameterName = "@distance";
                paramDistanta.Value = entity.Distance.ToString();
                comm.Parameters.Add(paramDistanta);

                var paramStil = comm.CreateParameter();
                paramStil.ParameterName = "@style";
                paramStil.Value = entity.Style.ToString();
                comm.Parameters.Add(paramStil);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No proba added !");
            }

        }

        public void delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<SampleDTO> GroupByNumberOfSample()
        {
            List<SampleDTO> rezultat = new List<SampleDTO>();

            IDbConnection conn = DBUtils.getConnection();
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "select S.id,S.distance, S.style, count(P.id) as NrInscrisi from Sample S" +
                "    left join Registration R on R.sampleId = S.id" +
                "    left join Participant P on R.participantId = P.id" +
                "   group by S.id";

                using (var Data = com.ExecuteReader())
                {
                    while (Data.Read())
                    {
                        string idSample = Data.GetString(0);
                        String distantaProbaS = Data.GetString(1);
                        String stilProbaS = Data.GetString(2);
                        int numberParticipants = Data.GetInt32(3);
                        SampleDTO proba = new SampleDTO((Distance)Enum.Parse(typeof(Distance), distantaProbaS), (Style)Enum.Parse(typeof(Style), stilProbaS), numberParticipants)
                        {
                            Id = idSample
                        };
                        rezultat.Add(proba);

                    }
                }
            }
            return rezultat;

        }

        public string FindMaxId()
        {

            List<string> lista = new List<string>();
            foreach (Sample p in findAll())
            {
                lista.Add(p.Id);
            }

            int max = 0;
            foreach (string s in lista)
            {

                int id = Int32.Parse(s);
                if (id > max)
                {
                    max = id;

                }

            }
            int maximBun = max + 1;
            string myString = maximBun.ToString();

            return myString;
        }



        public string findIdSample(Distance d, Style s)
        {
            IDbConnection conn = DBUtils.getConnection();
            using (var comm = conn.CreateCommand())
            {
                comm.CommandText = "select s.id from Sample s where s.distance=@distance and s.style=@style";

                var paramDistanta = comm.CreateParameter();
                paramDistanta.ParameterName = "@distance";
                paramDistanta.Value = d.ToString();
                comm.Parameters.Add(paramDistanta);

                var paramStil = comm.CreateParameter();
                paramStil.ParameterName = "@style";
                paramStil.Value = s.ToString();
                comm.Parameters.Add(paramStil);

                using (var Data = comm.ExecuteReader())
                {
                    if (Data.Read())
                    {
                        string id = Data.GetString(0);
                        return id;
                    }
                }
            }
            return null;

        }
    }

}
