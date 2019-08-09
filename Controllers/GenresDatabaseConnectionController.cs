using Biblioteka.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	internal static class GenresDatabaseConnectionController
	{
		/// <summary>
		/// Returns a list of all genres in the database
		/// </summary>
		/// <returns></returns>
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
							Nazwa = reader.GetString(1).Trim()
						};
						genres.Add(genre);
					}
				}
			}

			connection.Close();

			return genres;
		}

		/// <summary>
		/// Returns a GenreModel for a given genre name
		/// </summary>
		/// <param name="name">Name of the genre to look for</param>
		/// <returns></returns>
		public static GenreModel GetGenreByName(string name)
		{
			var genre = new GenreModel();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Gatunki WHERE Genre='{name}'", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					genre.Id = reader.GetInt32(0);
					genre.Nazwa = name;
				}
			}

			connection.Close();

			return genre;
		}

		/// <summary>
		/// Returns a GenreModel for the specified Id
		/// </summary>
		/// <param name="id">The Id of genre to look for</param>
		/// <returns></returns>
		public static GenreModel GetGenreById(int id)
		{
			GenreModel genre = null;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Gatunki WHERE Id={id}", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					genre = new GenreModel()
					{
						Id = id,
						Nazwa = reader.GetString(1)
					};
				}
			}

			connection.Close();

			return genre;
		}
	}
}
