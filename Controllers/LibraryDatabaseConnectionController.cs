using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	internal static class LibraryDatabaseConnectionController
	{
		/// <summary>
		/// Returns a list of all borrowings of currently logged in user
		/// </summary>
		/// <returns></returns>
		public static List<BorrowingModel> RetrieveBorrowingsForUser()
		{
			var listaWypożyczeń = new List<BorrowingModel>();

			//get user ID
			if (UserModel.CurrentUser == null)
			{
				throw new UserNotLoggedInException();
			}

			var userId = UserModel.CurrentUser.Id;

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

						var bookId = reader.GetInt32(1);

						//get the book's title and its author name

						var getBook = new SqlCommand($"SELECT * FROM Książki WHERE Id={bookId}", connection);
						using (var reader2 = getBook.ExecuteReader())
						{
							if (reader2.HasRows)
							{
								while (reader2.Read())
								{
									wypożyczenie.TytułKsiążki = reader2.GetString(3).Trim();
									var authorId = reader2.GetInt32(1);

									var getAuthor = new SqlCommand($"SELECT * FROM Autorzy WHERE Id={authorId}", connection);
									using (var reader3 = getAuthor.ExecuteReader())
									{
										if (reader3.HasRows)
										{
											while (reader3.Read())
											{
												wypożyczenie.NazwaAutora = reader3.GetString(1).Trim() + reader3.GetString(2).Trim();
											}
										}
									}
								}
							}
						}

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

		/// <summary>
		/// Returns the most recent borrowing of currently logged in user
		/// </summary>
		/// <returns></returns>
		public static BorrowingModel RetrieveRecentBorrowing()
		{
			BorrowingModel ostatnieWypożyczenie = null;

			var userId = UserModel.CurrentUser.Id;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Wypożycznia WHERE UserId={userId}", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (reader.HasRows)
						{
							ostatnieWypożyczenie = new BorrowingModel()
							{
								UserId = userId,
								BookId = reader.GetInt32(1),
								DataWypożyczenia = reader.GetDateTime(3),
								TerminOddania = reader.GetDateTime(4)
							};
						}
					}
				}
				else
				{
					throw new NoBorrowingFoundException();
				}
			}

			connection.Close();

			return ostatnieWypożyczenie;
		}

		/// <summary>
		/// Returns a list of soon-ending (within two days from now) borrowings of currently logged in user
		/// </summary>
		/// <returns></returns>
		public static List<BorrowingModel> RetrieveEndingBorrowings()
		{
			var endingBorrowings = new List<BorrowingModel>();
			var allBorrowings = RetrieveBorrowingsForUser();

			foreach (var book in allBorrowings)
			{
				var date = book.TerminOddania;

				if (date > DateTime.Now && date < DateTime.Now.AddDays(2))
				{
					endingBorrowings.Add(book);
				}
			}

			return endingBorrowings;
		}

		/// <summary>
		/// Marks the chosen book as borrowed if the user has less than five borrowings
		/// </summary>
		/// <param name="bookModel">BookModel of the borrowed book</param>
		public static void BorrowBook(BookModel bookModel)
		{
			if (UserModel.CurrentUser == null)
			{
				throw new UserNotLoggedInException();
			}

			if (bookModel.IsBorrowed)
			{
				throw new BookBorrowedException();
			}

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			//Check if the user has no more than 5 borrowings (that's the limit at a time)

			var amountOfBorrowings = new SqlCommand($"SELECT COUNT(*) FROM Wypożyczenia WHERE ReaderId={UserModel.CurrentUser.Id}", connection);

			var amount = (int)(amountOfBorrowings.ExecuteScalar());

			if (amount >= 5)
			{
				throw new TooManyBorrowings();
			}


			var borrowing = new BorrowingModel()
			{
				UserId = UserModel.CurrentUser.Id,
				BookId = bookModel.Id,
				DataWypożyczenia = DateTime.Now,
				TerminOddania = DateTime.Now.AddDays(14)
			};

			var addBorrowing = new SqlCommand($"INSERT INTO Wypożyczenia (BookId, ReaderId, StartDate, EndDate) VALUES " +
				$"(@BookId, @ReaderId, @StartDate, @EndDate)", connection);

			addBorrowing.Parameters.AddWithValue("@BookId", borrowing.BookId);
			addBorrowing.Parameters.AddWithValue("@ReaderId", borrowing.UserId);
			addBorrowing.Parameters.AddWithValue("@StartDate", borrowing.DataWypożyczenia);
			addBorrowing.Parameters.AddWithValue("@EndDate", borrowing.TerminOddania);

			addBorrowing.ExecuteNonQuery();

			connection.Close();
		}

		/// <summary>
		/// Extend a borrowing by an additional week
		/// </summary>
		/// <param name="borrowing">BorrowingModel to extend return time for</param>
		public static void ExtendUserBorrowing(BorrowingModel borrowing)
		{
			if (UserModel.CurrentUser == null)
			{
				throw new UserNotLoggedInException();
			}

			var userId = UserModel.CurrentUser.Id;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var getEndDate = new SqlCommand($"SELECT * FROM Wypożyczenia WHERE BookId={borrowing.Książka.Id}", connection);

			connection.Open();

			using (var reader = getEndDate.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					var previosuDate = reader.GetDateTime(4);
					var extendedDate = previosuDate.AddDays(7);

					var command = new SqlCommand($"UPDATE Wypożyczenia SET EndDate={extendedDate} WHERE BookId={borrowing.Książka.Id}", connection);

					command.ExecuteNonQuery();
				}
				else
				{
					throw new NoBorrowingFoundException();
				}
			}

			connection.Close();
		}

		/// <summary>
		/// Returns a list of all borrowings in the database
		/// </summary>
		/// <returns></returns>
		public static List<BorrowingModel> GetAllBorrowings()
		{
			var wypożyczenia = new List<BorrowingModel>();
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Wypożyczenia", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var borrowing = new BorrowingModel()
						{
							Id = reader.GetInt32(0),
							BookId = reader.GetInt32(1),
							UserId = reader.GetInt32(2),
							DataWypożyczenia = reader.GetDateTime(3),
							TerminOddania = reader.GetDateTime(4)
						};

						wypożyczenia.Add(borrowing);
					}
				}
			}

			connection.Close();

			return wypożyczenia;
		}

		/// <summary>
		/// Returns a borrowing, making the book available and reducing user's amount of concurrent borrowings
		/// </summary>
		/// <param name="borrowing">BorrowingModel to return</param>
		public static void ReturnBook(BorrowingModel borrowing)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"DELETE FROM Wypożyczenia WHERE BookId={borrowing.Książka.Id}", connection);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
	}
}
