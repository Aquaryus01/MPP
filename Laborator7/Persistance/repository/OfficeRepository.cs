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
    public class OfficeRepository : IOfficeRepository
    {
        public static readonly ILog logger = LogManager.GetLogger("OfficeRepository");

        public OfficeRepository()
        {
            logger.Info("Creating the Office Repository");
        }

        public void delete(string id)
        {
            logger.InfoFormat("Se sterge oficiul cu id-ul {0}", id);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "delete from Office where id=@id";

                IDbDataParameter paramId = com.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                com.Parameters.Add(paramId);
                var result = com.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.Info("Eroare incercand sa se stearga oficiul cu id-ul");
                    throw new Exception("Eroare la stergere");
                }
                logger.InfoFormat("S-a sters angajatul cu id-ul {0}", id);
            }

        }

        public List<Office> findAll()
        {
            IDbConnection connection = DBUtils.getConnection();
            List<Office> oficiuList = new List<Office>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select id, user, password from Office";

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        string id = result.GetString(0);
                        string username = result.GetString(1);
                        string password = result.GetString(2);
                        Office oficiu = new Office(username, password)
                        {
                            Id = id
                        };
                        oficiuList.Add(oficiu);
                    }
                }
            }

            return oficiuList;
        }

        public Office findOne(string id)
        {
            logger.InfoFormat("Se cauta angajatul cu id-ul {0}", id);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "select id,user,password from Office where id=@id";
                IDbDataParameter paramId = com.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                com.Parameters.Add(paramId);

                using (var Data = com.ExecuteReader())
                {
                    if (Data.Read())
                    {

                        string username = Data.GetString(0);
                        string password = Data.GetString(1);
                        Office office = new Office(username, password)
                        {
                            Id = id
                        };
                        logger.InfoFormat("S-a gasit angajatul cu id-ul {0}", office.Id);
                        return office;
                    }
                }
            }
            logger.InfoFormat("Nu s a gasit officiul cu id ul {0}", id);
            return null;
        }

        public void save(Office entity)
        {
            logger.InfoFormat("Saving entity {0}", entity);
            IDbConnection connection = DBUtils.getConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "insert into Office values (@id, @user, @password)";

                var paramId = command.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                command.Parameters.Add(paramId);

                var paramUser = command.CreateParameter();
                paramUser.ParameterName = "@user";
                paramUser.Value = entity.Username;
                command.Parameters.Add(paramUser);

                var paramPass = command.CreateParameter();
                paramPass.ParameterName = "@password";
                paramPass.Value = entity.Password;
                command.Parameters.Add(paramPass);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No entity added!");
            }
        }


        public bool Login(string username, string password)
        {
            logger.InfoFormat("Se cauta daca se poate efectua logarea angajatului {0}", username);

            IDbConnection conn = DBUtils.getConnection();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "select id from Office where user=@user and password=@pass";

                IDbDataParameter user = com.CreateParameter();
                user.ParameterName = "@user";
                user.Value = username;
                com.Parameters.Add(user);

                IDbDataParameter pass = com.CreateParameter();
                pass.ParameterName = "@pass";
                pass.Value = password;
                com.Parameters.Add(pass);

                using (var Data = com.ExecuteReader())
                {
                    if (Data.Read())
                    {
                        return true;
                    }
                }

            }
            return false;
        }

    }
}
