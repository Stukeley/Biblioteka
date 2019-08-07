using Biblioteka.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Controllers
{
	internal static class AuthorsDatabaseConnectionController
	{
		/// <summary>
		/// Returns a list of all authors in the database
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Returns an author model based on the provided Name and Surname
		/// </summary>
		/// <param name="name">Name of the author to return</param>
		/// <param name="surname">Surname of the author to return</param>
		/// <returns></returns>
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

		/// <summary>
		/// Returns a list of three most recently added authors
		/// </summary>
		/// <returns></returns>
		public static List<AuthorModel> GetRecentAuthors()
		{
			//3 latest authors

			var allAuthors = GetAllAuthors();

			var recentAuthors = (from author in allAuthors orderby author.Id descending select author).Take(3).ToList();

			return recentAuthors;
		}

		/// <summary>
		/// Returns a list of filtered authors, based on either the Name, or the Surname, or both
		/// </summary>
		/// <param name="name">Optional name of the Author to look for</param>
		/// <param name="surname">Optional surname of the Author to look for</param>
		/// <returns></returns>
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

		/// <summary>
		/// Returns biography of the author tied to the provided Id
		/// </summary>
		/// <param name="id">Id of the author to retrieve biography for</param>
		/// <returns></returns>
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
