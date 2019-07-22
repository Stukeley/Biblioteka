using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Biblioteka.Controllers
{
	/// <summary>
	/// This class is used for connecting with the database and retrieving user information, or inserting new users into it
	/// </summary>
	internal class UserDatabaseConnectionController
	{
		/// <summary>
		/// This method will check for the user assigned to the provided email in the database, and will check if passwords match.
		/// In such case, it returns the user
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

			//?
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
						var dbName = reader["Name"].ToString();
						var dbSurname = reader["Surname"].ToString();
						var dbId = int.Parse(reader["Id"].ToString());

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

						Biblioteka.Windows.LoginWindow.IsLoggedIn = true;
						var userModel = new UserModel(dbName, dbSurname, email, password)
						{
							UserId = dbId
						};
						UserModel.CurrentUser = userModel;
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
					var dbName = reader["Name"].ToString();
					var dbSurname = reader["Surname"].ToString();

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
			connection.Close();

			UserModel.CurrentUser = new UserModel(name, surname, email, password, currentDate);
		}
	}
}
