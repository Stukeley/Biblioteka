﻿using Biblioteka.Models;
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
									Imię = authorReader.GetString(1).Trim(),
									Nazwisko = authorReader.GetString(2).Trim(),
									DataUrodzenia = authorReader.GetDateTime(3),
									Biografia = authorReader.GetString(4).Trim()
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
									Nazwa = genreReader.GetString(1).Trim()
								};
							}
						}

						var book = new BookModel()
						{
							Id = reader.GetInt32(0),
							Autor = author,
							Gatunek = genre,
							Tytuł = reader.GetString(3).Trim()
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
			//3 latest books

			var allBooks = GetAllBooks();

			var recentBooks = (from book in allBooks orderby book.Id descending select book).Take(3).ToList();

			return recentBooks;
		}

		public static List<BookModel> GetUnborrowedBooks()
		{
			var allBooks = GetAllBooks();

			var books = allBooks.Where(x => x.IsBorrowed == false).ToList();

			return books;
		}

		public static int? GetBookByModel(BookModel bookModel)
		{
			//null means book not found
			int? id = null;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"SELECT * FROM Książki WHERE Title='{bookModel.Tytuł}'", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					id = reader.GetInt32(0);
				}
			}

			return id;
		}

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
	}
}
