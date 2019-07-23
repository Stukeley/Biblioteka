using Biblioteka.Models;
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

			var command = new SqlCommand($"INSERT INTO BibliotekaDB.Gatunki (Genre) VALUES (@Genre)", connection);

			command.Parameters.AddWithValue("@Genre", genre.Nazwa);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}

		public static void InsertAuthorIntoDatabase(AuthorModel author)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"INSERT INTO BibliotekaDB.Autorzy (Name, Surname, DateOfBirth, Biography) VALUES " +
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

			var getAuthorId = new SqlCommand($"SELECT Id FROM BibliotekaDB.Autorzy WHERE Name='{book.Autor.Imię}' AND Surname='{book.Autor.Nazwisko}'" +
				$"AND DateOfBirth={book.Autor.DataUrodzenia}", connection);
			var authorId = getAuthorId.ExecuteNonQuery();

			var getGenreId = new SqlCommand($"SELECT Id FROM BibliotekaDB.Gatunki WHERE Genre='{book.Gatunek.Nazwa}'", connection);
			var genreId = getGenreId.ExecuteNonQuery();

			var command = new SqlCommand($"INSERT INTO BibliotekaDB.Książki (AuthorId, GenreId, Title) VALUES (@AuthorId, @GenreId, @Title)", connection);

			command.Parameters.AddWithValue("@AuthorId", authorId);
			command.Parameters.AddWithValue("@GenreId", genreId);
			command.Parameters.AddWithValue("@Title", book.Tytuł);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
	}
}
