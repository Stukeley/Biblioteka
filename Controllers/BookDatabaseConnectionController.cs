using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Biblioteka.Controllers
{
	internal static class BookDatabaseConnectionController
	{
		/// <summary>
		/// Returns a list of all books in the database
		/// </summary>
		/// <returns></returns>
		public static List<BookModel> GetAllBooks()
		{
			var books = new List<BookModel>();

			//BookModel, authorId, genreId
			var authorAndGenreIds = new List<Tuple<BookModel, int, int>>();

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
						var book = new BookModel()
						{
							Id = reader.GetInt32(0),
							Tytuł = reader.GetString(3).Trim()
						};
						var authorId = reader.GetInt32(1);

						var genreId = reader.GetInt32(2);

						authorAndGenreIds.Add(new Tuple<BookModel, int, int>(book, authorId, genreId));
					}
				}
			}

			foreach (var elem in authorAndGenreIds)
			{
				AuthorModel author = null;
				var getAuthorModel = new SqlCommand($"SELECT * FROM Autorzy WHERE Id={elem.Item2}", connection);
				using (var authorReader = getAuthorModel.ExecuteReader())
				{
					if (authorReader.HasRows)
					{
						authorReader.Read();
						author = new AuthorModel()
						{
							Id = elem.Item2,
							Imię = authorReader.GetString(1).Trim(),
							Nazwisko = authorReader.GetString(2).Trim(),
							DataUrodzenia = authorReader.GetDateTime(3),
							Biografia = authorReader.GetString(4).Trim()
						};
					}
				}


				GenreModel genre = null;
				var getGenreModel = new SqlCommand($"SELECT * FROM Gatunki WHERE Id={elem.Item3}", connection);
				using (var genreReader = getGenreModel.ExecuteReader())
				{
					if (genreReader.HasRows)
					{
						genreReader.Read();
						genre = new GenreModel()
						{
							Id = elem.Item3,
							Nazwa = genreReader.GetString(1).Trim()
						};
					}
				}

				elem.Item1.Autor = author;

				elem.Item1.Gatunek = genre;

				books.Add(elem.Item1);
			}

			connection.Close();

			return books;
		}

		/// <summary>
		/// Returns a list of three most recently added books
		/// </summary>
		/// <returns></returns>
		public static List<BookModel> GetRecentBooks()
		{
			//3 latest books

			var allBooks = GetAllBooks();

			var recentBooks = (from book in allBooks orderby book.Id descending select book).Take(3).ToList();

			return recentBooks;
		}

		/// <summary>
		/// Returns a list of books that are available right now
		/// </summary>
		/// <returns></returns>
		public static List<BookModel> GetUnborrowedBooks()
		{
			var allBooks = GetAllBooks();

			var books = allBooks.Where(x => x.IsBorrowed == false).ToList();

			return books;
		}

		/// <summary>
		/// Returns a list of filtered books, based on either the Author (name, surname) or the Title (or both)
		/// </summary>
		/// <param name="author">Optional name and/or surname of the Author to look for</param>
		/// <param name="title">Optional title of the Book to look for</param>
		/// <returns></returns>
		public static List<BookModel> FilterBooks(string author = null, string title = null)
		{
			var allBooks = GetAllBooks();
			List<BookModel> filteredBooks = null;

			if (author == null && title == null)
			{
				return allBooks;
			}

			//I'm kinda ashamed of the code below

			if (author != null && title == null)
			{
				var nameAndSurname = author.Split(new[] { ' ' });

				if (nameAndSurname.Length < 2)
				{
					filteredBooks = allBooks.Where(x => x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[0])).ToList();
				}
				else
				{
					filteredBooks = allBooks.Where(x => x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Imię.Contains(nameAndSurname[1])
				|| x.Autor.Nazwisko.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[1])).ToList();
				}
			}

			if (author == null && title != null)
			{
				filteredBooks = allBooks.Where(x => x.Tytuł.Contains(title)).ToList();
			}

			if (author != null && title != null)
			{
				var nameAndSurname = author.Split(new[] { ' ' });

				if (nameAndSurname.Length < 2)
				{
					filteredBooks = allBooks.Where(x => x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[0])).ToList();
				}
				else
				{
					filteredBooks = allBooks.Where(x => (x.Autor.Imię.Contains(nameAndSurname[0]) || x.Autor.Imię.Contains(nameAndSurname[1])
				|| x.Autor.Nazwisko.Contains(nameAndSurname[0]) || x.Autor.Nazwisko.Contains(nameAndSurname[1])) && (x.Tytuł.Contains(title))).ToList();
				}
			}

			return filteredBooks;
		}

		/// <summary>
		/// Returns a BookModel for the specified Id
		/// </summary>
		/// <param name="id">The Id of book to look for</param>
		/// <returns></returns>
		public static BookModel GetBookById(int id)
		{
			BookModel book = null;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Książki WHERE Id={id}", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					book = new BookModel()
					{
						Id = id,
						Tytuł = reader.GetString(3),
						IsBorrowed = reader.GetBoolean(4),
						Autor = AuthorsDatabaseConnectionController.GetAuthorById(reader.GetInt32(1)),
						Gatunek = GenresDatabaseConnectionController.GetGenreById(reader.GetInt32(2))
					};
				}
			}

			connection.Close();

			return book;
		}
	}
}
