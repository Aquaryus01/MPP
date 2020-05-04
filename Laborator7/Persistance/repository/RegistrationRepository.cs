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

    public class RegistrationRepository : IRegistrationRepository
    {

        public static readonly ILog logger = LogManager.GetLogger("RegistrationRepository");

        public RegistrationRepository()
        {
            logger.Info("Creating the Office Repository");
        }

        public void delete(string id)
        {
            throw new NotImplementedException();

        }

        public List<Registration> findAll()
        {
            IDbConnection connection = DBUtils.getConnection();
            List<Registration> registrationList = new List<Registration>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select id, participantId, sampleId from Registration";

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        string id = result.GetString(0);
                        string participantId = result.GetString(1);
                        string sampleId = result.GetString(2);
                        Registration registration = new Registration(participantId, sampleId)
                        {
                            Id = id
                        };
                        registrationList.Add(registration);
                    }
                }
            }

            return registrationList;
        }

        public Registration findOne(string id)
        {
            throw new NotImplementedException();
        }

        public void save(Registration entity)
        {
            logger.InfoFormat("Se salveaza Inscrierea cu id-il {0}", entity.Id);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "insert into Registration values (@id,@participantId,@sampleId)";

                IDbDataParameter paramIdAngajat = com.CreateParameter();
                paramIdAngajat.ParameterName = "@id";
                paramIdAngajat.Value = entity.Id;
                com.Parameters.Add(paramIdAngajat);

                IDbDataParameter paramidPart = com.CreateParameter();
                paramidPart.ParameterName = "@participantId";
                paramidPart.Value = entity.ParticipantId;
                com.Parameters.Add(paramidPart);

                IDbDataParameter paramidCursa = com.CreateParameter();
                paramidCursa.ParameterName = "@sampleId";
                paramidCursa.Value = entity.SampleId;
                com.Parameters.Add(paramidCursa);

                var result = com.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.Info("Error while adding");
                    throw new Exception("Nici o inscriere adaugata!");
                }

            }
            logger.InfoFormat("A fost adaugat inscrierea cu id-ul {0}", entity.Id);
        }

        public string FindMaxId()
        {
            List<string> lista = new List<string>();
            foreach (Registration p in findAll())
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


        public List<String> findBySample(Distance distance, Style style)
        {

            List<String> rezultat = new List<String>();
            logger.Info("Se cauta id-ul maxim din Inscriere");
            IDbConnection conn = DBUtils.getConnection();


            using (var com = conn.CreateCommand())
            {
                com.CommandText = "select P.id from Participant P inner join Registration R on P.id = R.participantId inner join Sample S on R.sampleId = S.id where S.distance=@distance and S.style=@style";


                var paramDistance = com.CreateParameter();
                paramDistance.ParameterName = "@distance";
                paramDistance.Value = distance.ToString();
                com.Parameters.Add(paramDistance);



                var paramStyle = com.CreateParameter();
                paramStyle.ParameterName = "@style";
                paramStyle.Value = style.ToString();
                com.Parameters.Add(paramStyle);

                using (var Data = com.ExecuteReader())
                {
                    while (Data.Read())
                    {
                        string id = Data.GetString(0);
                        rezultat.Add(id.ToString());

                    }
                }
            }
            return rezultat;

        }
        public List<RegistrationDTO> findByParticipant(string p)
        {
            List<RegistrationDTO> participants = new List<RegistrationDTO>();
            logger.Info("Se cauta id-ul maxim din Inscriere");
            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "Select P.firstName, P.lastName, P.age,  S.distance, S.style from Participant P " +
                    " inner join Registration R on P.id = R.participantId " +
                    " inner join Sample S on R.sampleId = S.id " +
                    " where P.id=@id ";



                var paramId = com.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = p;
                com.Parameters.Add(paramId);


                using (var Data = com.ExecuteReader())
                {
                    while (Data.Read())
                    {
                        //int id = Data.GetInt32(0);
                        String firstName = Data.GetString(0);
                        String lastName = Data.GetString(1);
                        int age = Data.GetInt32(2);
                        string distance = Data.GetString(3);
                        string style = Data.GetString(4);
                        // int numberParticipants = Data.GetInt32(3);
                        RegistrationDTO registration = new RegistrationDTO(firstName, lastName, age, (Distance)Enum.Parse(typeof(Distance), distance), (Style)Enum.Parse(typeof(Style), style));
                        participants.Add(registration);

                    }
                }
            }
            return participants;

        }


        public string findIdRegistration(string idp, string ids)
        {
            IDbConnection conn = DBUtils.getConnection();
            using (var comm = conn.CreateCommand())
            {
                comm.CommandText = "select i.id from Registration i where participantId=@participantId and sampleId=@sampleId";


                var paramIdp = comm.CreateParameter();
                paramIdp.ParameterName = "@participantId";
                paramIdp.Value = idp;
                comm.Parameters.Add(paramIdp);

                var paramIds = comm.CreateParameter();
                paramIds.ParameterName = "@sampleId";
                paramIds.Value = ids;
                comm.Parameters.Add(paramIds);

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
