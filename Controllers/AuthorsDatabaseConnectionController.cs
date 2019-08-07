using Biblioteka.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Controllers
{
	internal static class AuthorsDatabaseConnectionController
	{
		public static List<AuthorModel> GetAllAuthors()
		{
			var authors = new List<AuthorModel>();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Autorzy", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var author = new AuthorModel()
						{
							Id = reader.GetInt32(0),
							Imię = reader.GetString(1).Trim(),
							Nazwisko = reader.GetString(2).Trim(),
							DataUrodzenia = reader.GetDateTime(3),
							Biografia = reader.GetString(4).Trim()
						};
						author.Nazwa = author.Imię + " " + author.Nazwisko;

						authors.Add(author);
					}
				}
			}

			connection.Close();

			return authors;
		}

		public static AuthorModel GetAuthorByName(string name, string surname)
		{
			var author = new AuthorModel();
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Autorzy WHERE Name='{name}' AND Surname='{surname}'", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					author.Id = reader.GetInt32(0);
					author.Imię = name;
					author.Nazwisko = surname;
					author.DataUrodzenia = reader.GetDateTime(3);
					author.Biografia = reader.GetString(4).Trim();
				}
			}

			connection.Close();
			return author;
		}

		public static List<AuthorModel> GetRecentAuthors()
		{
			//3 latest authors

			var allAuthors = GetAllAuthors();

			var recentAuthors = (from author in allAuthors orderby author.Id descending select author).Take(3).ToList();

			return recentAuthors;
		}

		public static List<AuthorModel> FilterAuthors(string name = null, string surname = null)
		{
			var allAuthors = GetAllAuthors();
			List<AuthorModel> filteredAuthors = null;

			if (name == null && surname == null)
			{
				return allAuthors;
			}

			if (name != null && surname == null)
			{
				filteredAuthors = allAuthors.Where(x => x.Imię.Contains(name)).ToList();
			}

			if (name == null && surname != null)
			{
				filteredAuthors = allAuthors.Where(x => x.Nazwisko.Contains(surname)).ToList();
			}

			if (name != null && surname != null)
			{
				filteredAuthors = allAuthors.Where(x => (x.Imię.Contains(name)) && (x.Nazwisko.Contains(surname))).ToList();
			}

			return filteredAuthors;
		}

		public static string GetAuthorBiography(int id)
		{
			string biography = "";

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT Biography FROM Autorzy WHERE Id={id}", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					biography = reader.GetString(0).Trim();
				}
			}

			connection.Close();

			return biography;
		}
	}
}
