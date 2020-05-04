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


    public class ParticipantRepository : IParticipantRepository
    {

        public static readonly ILog log = LogManager.GetLogger("ParticipantRepository");

        public ParticipantRepository() { log.Info("Initializing ParticipantRepository"); }


        public void delete(string id)
        {
            log.InfoFormat("Deleting entity with id {0}", id);
            IDbConnection connection = DBUtils.getConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "delete from Participant where id=@id";

                var paramId = command.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                command.Parameters.Add(paramId);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.Info("Eroare incercand sa se stearga participantul cu id-ul");
                    throw new Exception("Eroare la stergere");
                }
                log.InfoFormat("S-a sters participantul cu id-ul {0}", id);
            }
        }

        public List<Participant> findAll()
        {
            IDbConnection connection = DBUtils.getConnection();
            List<Participant> participantList = new List<Participant>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select id, firstName, lastName, age from Participant";

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        string id = result.GetString(0);
                        string firstName = result.GetString(1);
                        string lastName = result.GetString(2);
                        int age = result.GetInt32(3);
                        Participant participant = new Participant(firstName, lastName, age)
                        {
                            Id = id
                        };
                        participantList.Add(participant);
                    }
                }
            }

            return participantList;
        }

        public Participant findOne(string id)
        {
            log.InfoFormat("Se cauta participantul cu id-ul {0}", id);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "select id, firstName, lastName, age from Participant where id=@id";
                IDbDataParameter paramId = com.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                com.Parameters.Add(paramId);

                using (var Data = com.ExecuteReader())
                {
                    if (Data.Read())
                    {
                        string firstName = Data.GetString(0);
                        string lastName = Data.GetString(1);
                        int age = Data.GetInt32(2);
                        Participant participant = new Participant(firstName, lastName, age)
                        {
                            Id = id
                        };
                        log.InfoFormat("Exiting with entity found {0}", participant);
                        return participant;
                    }
                }
            }
            log.InfoFormat("Nu s a gasit participantul cu id ul {0}", id);
            return null;
        }



        public void save(Participant entity)
        {
            log.InfoFormat("Se salveaza participantul cu id-il {0}", entity.Id);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "insert into Participant values (@id,@firstName,@lastName, @age)";

                IDbDataParameter paramId = com.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                com.Parameters.Add(paramId);

                IDbDataParameter paramFirstName = com.CreateParameter();
                paramFirstName.ParameterName = "@firstName";
                paramFirstName.Value = entity.FirstName;
                com.Parameters.Add(paramFirstName);

                IDbDataParameter paramLastName = com.CreateParameter();
                paramLastName.ParameterName = "@lastName";
                paramLastName.Value = entity.LastName;
                com.Parameters.Add(paramLastName);


                IDbDataParameter paramAge = com.CreateParameter();
                paramAge.ParameterName = "@age";
                paramAge.Value = entity.Age;
                com.Parameters.Add(paramAge);

                var result = com.ExecuteNonQuery();
                if (result == 0)
                {
                    log.Info("Error while adding");
                    throw new Exception("Nici un participant adaugata!");
                }

            }
            log.InfoFormat("A fost adaugata participantul cu id-ul {0}", entity.Id);
        }

        public string FindMaxId()
        {

            List<string> lista = new List<string>();
            foreach (Participant p in findAll())
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

    }
}
