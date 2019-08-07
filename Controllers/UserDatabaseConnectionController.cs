using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	internal static class UserDatabaseConnectionController
	{
		/// <summary>
		/// This method will check for the user assigned to the provided email in the database, and will check if passwords match.
		/// In such case, it makes the user the CurrentUser
		/// </summary>
		/// <param name="email">The email of the user to look for</param>
		/// <param name="password">The password that has to match the one saved in the database</param>
		/// <returns></returns>
		public static void GetUserByEmail(string email, string password, bool? rememberLoginCredentials)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"SELECT * FROM Czytelnicy WHERE Email='{email}'", connection);

			connection.Open();

			using (var reader = command.ExecuteReader())
			{
				if (!reader.HasRows)
				{
					throw new EmailException();
				}
				else
				{
					reader.Read();
					var dbPassword = reader.GetString(4).Trim();

					if (dbPassword != password)
					{
						throw new PasswordException();
					}
					else
					{
						var dbName = reader["Name"].ToString().Trim();
						var dbSurname = reader["Surname"].ToString().Trim();
						var dbId = int.Parse(reader["Id"].ToString());
						var fees = int.Parse(reader["Fees"].ToString());

						if (rememberLoginCredentials == true)
						{
							Properties.Settings.Default.UserEmail = email;

							//TODO: hash the password
							Properties.Settings.Default.UserPassword = password;

							Properties.Settings.Default.Save();
						}
						else
						{
							if (!string.IsNullOrEmpty(Properties.Settings.Default.UserEmail) && !string.IsNullOrEmpty(Properties.Settings.Default.UserPassword))
							{
								Properties.Settings.Default.UserEmail = "";
								Properties.Settings.Default.UserPassword = "";
								Properties.Settings.Default.Save();
							}
						}

						var userModel = new UserModel()
						{
							Imię = dbName,
							Nazwisko = dbSurname,
							Email = email,
							Hasło = password,
							Id = dbId,
							Fees = fees,
							DateOfCreation = DateTime.Now
						};
						UserModel.CurrentUser = userModel;

						//check special privileges
						UserModel.CurrentUser.IsSpecialAccount = CheckUserPrivileges();
					}
				}
			}

			connection.Close();
		}

		/// <summary>
		/// This method will check if the provided email address exists in the database, and if so, if the name and surname of the users match. In such case,
		/// the old password will be updated with the new one.
		/// </summary>
		/// <param name="name">Name of the user</param>
		/// <param name="surname">Surname of the user</param>
		/// <param name="email">Email of the user</param>
		/// <param name="newPassword">New password of the user</param>
		public static void ResetUserPassword(string name, string surname, string email, string newPassword)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

			var connection = new SqlConnection(connString);

			var command = new SqlCommand($"SELECT * FROM Czytelnicy WHERE Email={email}", connection);

			connection.Open();

			//?
			using (var reader = command.ExecuteReader())
			{
				reader.Read();

				if (!reader.HasRows)
				{
					throw new UserNotFoundException();
				}
				else
				{
					var dbName = reader.GetString(1).Trim();
					var dbSurname = reader.GetString(2).Trim();

					if (dbName != name || dbSurname != surname)
					{
						throw new CredentialsException();
					}
					else
					{
						var updatePasswordCommand = new SqlCommand($"UPDATE Czytelnicy SET Password='{newPassword}' WHERE Email={email}",
							connection);
						updatePasswordCommand.ExecuteNonQuery();
					}
				}
			}

			connection.Close();
		}

		/// <summary>
		/// This method is used for registering new users in the database
		/// </summary>
		/// <param name="name">Name of the user</param>
		/// <param name="surname">Surname of the user</param>
		/// <param name="email">Email of the user</param>
		/// <param name="password">Password of the user</param>
		public static void RegisterUser(string name, string surname, string email, string password)
		{
			//check if email exists in the database
			var currentDate = DateTime.Now;

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand("INSERT INTO Czytelnicy (Name, Surname, Email, Password, DateOfCreation) VALUES " +
				"(@Name, @Surname, @Email, @Password, @DateOfCreation);",
				connection);

			command.Parameters.AddWithValue("@Name", name);
			command.Parameters.AddWithValue("@Surname", surname);
			command.Parameters.AddWithValue("@Email", email);
			command.Parameters.AddWithValue("@Password", password);
			command.Parameters.AddWithValue("@DateOfCreation", currentDate);

			command.ExecuteNonQuery();

			UserModel.CurrentUser = new UserModel()
			{
				Imię = name,
				Nazwisko = surname,
				Email = email,
				Hasło = password,
				DateOfCreation = currentDate
			};

			var getUserId = new SqlCommand($"SELECT Id FROM Czytelnicy WHERE Email='{email}'", connection);

			var id = (int)getUserId.ExecuteScalar();
			UserModel.CurrentUser.Id = id;

			connection.Close();
		}

		/// <summary>
		/// This method is responsible for updating user's credentials if requested
		/// </summary>
		/// <param name="name">New name of the user</param>
		/// <param name="surname">New surname of the user</param>
		/// <param name="email">New email of the user</param>
		/// <param name="password">New password of the user</param>
		public static void UpdateUserCredentials(string name, string surname, string email, string password)
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"UPDATE Czytelnicy SET Name='{name}', Surname='{surname}', Email='{email}', Password='{password}' " +
				$"WHERE Email='{UserModel.CurrentUser.Email}'", connection);

			command.ExecuteNonQuery();

			connection.Close();
		}

		/// <summary>
		/// This method is used for checking special privileges of the currently logged user. True means the user is an Admin (able to add new content
		/// like Authors or Books or Genres), false means the user is just a reader
		/// </summary>
		public static bool CheckUserPrivileges()
		{
			bool isSpecialAccount = false;
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT IsSpecialAccount FROM Czytelnicy WHERE Email='{UserModel.CurrentUser.Email}'", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					reader.Read();
					isSpecialAccount = reader.GetBoolean(0);
				}
			}

			connection.Close();

			return isSpecialAccount;
		}

		/// <summary>
		/// Returns a list of all users in the database
		/// </summary>
		/// <returns></returns>
		public static List<UserModel> GetAllUsers()
		{
			var użytkownicy = new List<UserModel>();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"SELECT * FROM Czytelnicy", connection);

			using (var reader = command.ExecuteReader())
			{
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var użytkownik = new UserModel()
						{
							Id = reader.GetInt32(0),
							Imię = reader.GetString(1).Trim(),
							Nazwisko = reader.GetString(2).Trim(),
							Email = reader.GetString(3).Trim(),
							Hasło = reader.GetString(4).Trim(),
							IsSpecialAccount = reader.GetBoolean(6)
						};

						użytkownicy.Add(użytkownik);
					}
				}
			}

			connection.Close();

			return użytkownicy;
		}

		/// <summary>
		/// Count overdue fees for currently borrowed books of all users
		/// </summary>
		public static void CountUserFees()
		{
			//(UserId, fee)
			var userIds = new Dictionary<int, int>();

			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			var allBorrowings = LibraryDatabaseConnectionController.GetAllBorrowings();

			foreach (var borrowing in allBorrowings)
			{
				if (borrowing.TerminOddania < DateTime.Now)
				{
					var amountOfWeeks = (int)(DateTime.Now - borrowing.TerminOddania).TotalDays / 7;
					var fee = amountOfWeeks * 5;
					LibraryDatabaseConnectionController.ExtendUserBorrowing(borrowing);
					userIds.Add(borrowing.UserId, fee);
				}
			}

			connection.Open();

			foreach (var pair in userIds)
			{
				var getCurrentFees = new SqlCommand($"SELECT Fees FROM Czytelnicy WHERE Id={pair.Key}");
				var currentFees = (int)(getCurrentFees.ExecuteScalar());

				var command = new SqlCommand($"UPDATE Czytelnicy SET Fees={currentFees + pair.Value} WHERE Id={pair.Key}", connection);
				command.ExecuteNonQuery();
			}

			connection.Close();
		}

		/// <summary>
		/// Clear currently-logged-in user's fees
		/// </summary>
		public static void ClearUserFees()
		{
			var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();
			var connection = new SqlConnection(connString);

			connection.Open();

			var command = new SqlCommand($"UPDATE Czytelnicy SET Fees=0 WHERE Id={UserModel.CurrentUser.Id}", connection);
			command.ExecuteNonQuery();

			connection.Close();
		}
	}
}
