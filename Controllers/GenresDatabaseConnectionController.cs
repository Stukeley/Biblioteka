using Biblioteka.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	internal static class GenresDatabaseConnectionController
	{
		public static List<GenreModel> GetAllGenres()
		{
			var genres = new List<GenreModel>();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Gatunki", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var genre = new GenreModel()
						{
							Id = reader.GetInt32(0),
							Nazwa = reader.GetString(1)
						};
						genres.Add(genre);
					}
				}
			}

			connection.Close();

			return genres;
		}
	}
}
