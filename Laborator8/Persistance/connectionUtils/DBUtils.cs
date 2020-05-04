using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Reflection;

namespace Persistance.connectionUtils
{
	public class DBUtils
	{
		private static IDbConnection instance = null;

		public static IDbConnection getConnection()
		{
			if (instance == null || instance.State == ConnectionState.Closed)
			{
				instance = getNewConnection();
				instance.Open();
			}
			return instance;
		}

		private static IDbConnection getNewConnection()
		{

			String connectionString =
				System.Configuration.ConfigurationManager.ConnectionStrings["InotDB"].ConnectionString;
			return new SqliteConnection(connectionString);

		}
	}
}
