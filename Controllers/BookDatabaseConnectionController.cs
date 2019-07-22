using Biblioteka.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Controllers
{
	internal static class BookDatabaseConnectionController
	{
		public static List<BookModel> GetAllBooks()
		{
			var books = new List<BookModel>();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Książki", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						AuthorModel author = null;
						var authorId = reader.GetInt32(1);

						var getAuthorModel = new SqlCommand($"SELECT * FROM Autorzy WHERE Id={authorId}", connection);

						using (var authorReader = getAuthorModel.ExecuteReader())
						{
							if (authorReader.HasRows)
							{
								authorReader.Read();
								author = new AuthorModel()
								{
									Id = authorId,
									Imię = authorReader.GetString(1),
									Nazwisko = authorReader.GetString(2),
									DataUrodzenia = authorReader.GetDateTime(3),
									Biografia = authorReader.GetString(4)
								};
							}
						}

						GenreModel genre = null;
						var genreId = reader.GetInt32(2);

						var getGenreModel = new SqlCommand($"SELECT * FROM Gatunki WHERE Id={genreId}", connection);

						using (var genreReader = getGenreModel.ExecuteReader())
						{
							if (genreReader.HasRows)
							{
								genreReader.Read();
								genre = new GenreModel()
								{
									Id = genreId,
									Nazwa = genreReader.GetString(1)
								};
							}
						}

						var book = new BookModel()
						{
							Id = reader.GetInt32(0),
							Autor = author,
							Gatunek = genre,
							Tytuł = reader.GetString(3)
						};

						books.Add(book);
					}
				}
			}

			connection.Close();

			return books;
		}

		public static List<BookModel> GetRecentBooks()
		{
			//3 latests books

			var allBooks = GetAllBooks();

			var recentBooks = (from book in allBooks orderby book.Id descending select book).Take(3).ToList();

			return recentBooks;
		}

		public static List<BookModel> FilterBooks(string author = null, string title = null)
		{
			var allBooks = GetAllBooks();
			List<BookModel> filteredBooks = null;

			if (author == null & title == null)
			{
				return allBooks;
			}

			if (author != null && title == null)
			{
				var nameAndSurname = author.Split(new[] { ' ' });

				filteredBooks = allBooks.Where(x => x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Imię.Contains(nameAndSurname[1])
				|| x.Autor.Nazwisko.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[1])).ToList();
			}

			if (author == null && title != null)
			{
				filteredBooks = allBooks.Where(x => x.Tytuł.Contains(title)).ToList();
			}

			if (author != null && title != null)
			{
				var nameAndSurname = author.Split(new[] { ' ' });

				filteredBooks = allBooks.Where(x => (x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Imię.Contains(nameAndSurname[1])
				|| x.Autor.Nazwisko.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[1])) && (x.Tytuł.Contains(title))).ToList();
			}

			return filteredBooks;
		}
	}
}
