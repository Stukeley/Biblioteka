using Biblioteka.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	internal static class DataInsertionController
	{
		public static void InsertGenreIntoDatabase(GenreModel genre)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"INSERT INTO Gatunki (Genre) VALUES (@Genre)", connection);

			command.Parameters.AddWithValue("@Genre", genre.Nazwa);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}

		public static void InsertAuthorIntoDatabase(AuthorModel author)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"INSERT INTO Autorzy (Name, Surname, DateOfBirth, Biography) VALUES " +
				$"(@Name, @Surname, @DateOfBirth, @Biography)", connection);

			command.Parameters.AddWithValue("@Name", author.Imię);
			command.Parameters.AddWithValue("@Surname", author.Nazwisko);
			command.Parameters.AddWithValue("@DateOfBirth", author.DataUrodzenia);
			command.Parameters.AddWithValue("@Biography", author.Biografia);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}

		public static void InsertBookIntoDatabase(BookModel book)
		{
			//find the genre ID and author ID in the database

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var getAuthorId = new SqlCommand($"SELECT Id FROM Autorzy WHERE Name='{book.Autor.Imię}' AND Surname='{book.Autor.Nazwisko}'" +
				$"AND DateOfBirth={book.Autor.DataUrodzenia}", connection);
			var authorId = getAuthorId.ExecuteNonQuery();

			var getGenreId = new SqlCommand($"SELECT Id FROM Gatunki WHERE Genre='{book.Gatunek.Nazwa}'", connection);
			var genreId = getGenreId.ExecuteNonQuery();

			var command = new SqlCommand($"INSERT INTO Książki (AuthorId, GenreId, Title) VALUES (@AuthorId, @GenreId, @Title)", connection);

			command.Parameters.AddWithValue("@AuthorId", authorId);
			command.Parameters.AddWithValue("@GenreId", genreId);
			command.Parameters.AddWithValue("@Title", book.Tytuł);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}

		/// <summary>
		/// This method will return a tuple containing the amounts of Books, Authors, Genres and Users in the database (in this order)
		/// </summary>
		/// <returns></returns>
		public static Tuple<int, int, int, int> GetAmountsOfItems()
		{
			int books, authors, genres, users;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var getBooks = new SqlCommand($"SELECT COUNT(*) FROM Książki", connection);
			var getAuthors = new SqlCommand($"SELECT COUNT(*) FROM Autorzy", connection);
			var getGenres = new SqlCommand($"SELECT COUNT(*) FROM Gatunki", connection);
			var getUsers = new SqlCommand($"SELECT COUNT(*) FROM Czytelnicy", connection);

			books = (int)getBooks.ExecuteScalar();
			authors = (int)getAuthors.ExecuteScalar();
			genres = (int)getGenres.ExecuteScalar();
			users = (int)getUsers.ExecuteScalar();

			connection.Close();

			return new Tuple<int, int, int, int>(books, authors, genres, users);
		}
	}
}
