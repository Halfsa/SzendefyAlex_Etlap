using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SzendefyAlex_Etlap
{
	public class EtlapService
	{
		MySqlConnection connection;
		public EtlapService() 
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
			builder.Server = "localhost";
			builder.Port = 3306;
			builder.UserID = "root";
			builder.Password = "";
			builder.Database = "etlapdb";

			connection = new MySqlConnection(builder.ConnectionString);
		}
		public List<Etlap> GetAllASC()
		{
			List<Etlap> kajak = new List<Etlap>();
			OpenConnection();
            string sql = "SELECT id, nev, kategoria, ar FROM etlap ORDER BY kategoria, nev ASC";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					Etlap kaja = new Etlap();
					kaja.id = reader.GetInt32("id");
					kaja.nev = reader.GetString("nev");
					kaja.kategoria = reader.GetString("kategoria");
					kaja.ar = reader.GetInt32("ar");
                    if (kaja.nev.Trim() != "")
                    {
						kajak.Add(kaja);
					}
                   
				}
			}
			CloseConnection();
			return kajak;
		}
		public List<Etlap> GetAllDESC()
		{
			List<Etlap> kajak = new List<Etlap>();
			OpenConnection();
			string sql = "SELECT id, nev, kategoria, ar FROM etlap ORDER BY kategoria, nev DESC";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					Etlap kaja = new Etlap();
					kaja.id = reader.GetInt32("id");
					kaja.nev = reader.GetString("nev");
					kaja.kategoria = reader.GetString("kategoria");
					kaja.ar = reader.GetInt32("ar");
					if (kaja.nev.Trim() != "")
					{
						kajak.Add(kaja);
					}

				}
			}
			CloseConnection();
			return kajak;
		}
		public string GetDescription(int id)
		{
			string description;
			OpenConnection();
			string sql = "SELECT leiras FROM etlap WHERE id = @id";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText= sql;
			command.Parameters.Add("@id",MySqlDbType.Int32);
			command.Parameters["@id"].Value = id;
			MySqlDataReader reader = command.ExecuteReader();
			reader.Read();
			description = reader.GetString("leiras");
			CloseConnection();
			return description;
		}

		private void CloseConnection()
		{
            if (connection.State == System.Data.ConnectionState.Open)
            {
				connection.Close();
            }
        }

		private void OpenConnection()
		{
			if (connection.State == System.Data.ConnectionState.Closed)
			{
				connection.Open();
			}
		}
		public bool Create(Etlap etlap, string leiras)
		{
			OpenConnection();
			string sql = $"INSERT INTO etlap (nev,leiras,ar,kategoria) VALUES (@NEV,@LEIRAS,@AR,@KATEGORIA)";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@NEV", MySqlDbType.String);
			command.Parameters["@NEV"].Value = etlap.nev;
			command.Parameters.Add("@LEIRAS", MySqlDbType.String);
			command.Parameters["@LEIRAS"].Value = leiras;
			command.Parameters.Add("@AR", MySqlDbType.Int32);
			command.Parameters["@AR"].Value = etlap.ar;
			command.Parameters.Add("@KATEGORIA", MySqlDbType.String);
			command.Parameters["@KATEGORIA"].Value = etlap.kategoria;
			if (command.ExecuteNonQuery() == 1)
			{
				CloseConnection();
				return true;
			}
			else
			{
				CloseConnection();
				return false;
			}

		}
		public bool Delete(int id)
		{
			OpenConnection();
			string sql = $"DELETE FROM etlap WHERE id = @ID";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@ID", MySqlDbType.Int32);
			command.Parameters["@ID"].Value = id;
			int result = command.ExecuteNonQuery();

			return result == 1;
		}
		public bool ModositMindSzazalek(int szam)
		{
			OpenConnection();
			string sql = "UPDATE etlap SET ar = ar*@AR";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@AR", MySqlDbType.String);
			command.Parameters["@AR"].Value = "1."+szam;
			int result = command.ExecuteNonQuery();
			return result >= 1;
		}
		public bool ModositEgySzazalek(int szam, int id)
		{
			OpenConnection();
			string sql = "UPDATE etlap SET ar = ar*@AR WHERE id = @ID";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@AR", MySqlDbType.String);
			command.Parameters["@AR"].Value = "1." + szam;
			command.Parameters.Add("@ID", MySqlDbType.Int32);
			command.Parameters["@ID"].Value = id;
			int result = command.ExecuteNonQuery();
			return result == 1;
		}
		public bool ModositMindForint(int szam)
		{
			OpenConnection();
			string sql = "UPDATE etlap SET ar = ar+@AR";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@AR", MySqlDbType.String);
			command.Parameters["@AR"].Value = szam;
			int result = command.ExecuteNonQuery();
			return result >= 1;
		}
		public bool ModositEgyForint(int szam, int id)
		{
			OpenConnection();
			string sql = "UPDATE etlap SET ar = ar+@AR WHERE id = @ID";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.Add("@AR", MySqlDbType.String);
			command.Parameters["@AR"].Value = szam;
			command.Parameters.Add("@ID", MySqlDbType.Int32);
			command.Parameters["@ID"].Value = id;
			int result = command.ExecuteNonQuery();
			return result == 1;
		}
	}
}
