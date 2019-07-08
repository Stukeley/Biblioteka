using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	/// <summary>
	/// This class is used for the Wypożyczenia (borrowing) table that joins the Readers and Books tables to create a functioning library
	/// </summary>
	internal class LibraryDatabaseConnectionController
	{
		public static List<BorrowingModel> RetrieveBorrowingsForUser()
		{
			var listaWypożyczeń = new List<BorrowingModel>();

			//get user ID
			if (UserModel.CurrentUser == null)
			{
				throw new UserNotLoggedInException();
			}

			var userId = UserModel.CurrentUser.UserId;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var getUserBorrowings = new SqlCommand($"SELECT * FROM Wypożyczenia WHERE ReaderId={userId}", connection);

			using (var reader = getUserBorrowings.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var wypożyczenie = new BorrowingModel()
						{
							BookId = reader.GetInt32(1),
							UserId = reader.GetInt32(2),
							DataWypożyczenia = reader.GetDateTime(3),
							TerminOddania = reader.GetDateTime(4)
						};

						listaWypożyczeń.Add(wypożyczenie);
					}
				}
				else
				{
					throw new NoBorrowingFoundException();
				}
			}

			connection.Close();

			return listaWypożyczeń;
		}

		public static void ExtendUserBorrowing()
		{
			if (UserModel.CurrentUser == null)
			{
				throw new UserNotLoggedInException();
			}

			var userId = UserModel.CurrentUser.UserId;

			var currentDate = DateTime.Now;
			var extendedDate = currentDate.AddDays(7);

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"UPDATE Wypożyczenia SET EndDate={extendedDate} WHERE ReaderId={userId}", connection);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}

		public static void ReturnBook(int bookId)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"DELETE FROM Wypożyczenia WHERE BookId={bookId}", connection);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
	}
}
