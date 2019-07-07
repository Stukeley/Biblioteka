using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for ForgotPasswordWindow.xaml
	/// </summary>
	public partial class ForgotPasswordWindow : Window
	{
		public ForgotPasswordWindow()
		{
			InitializeComponent();
		}

		private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
		{
			//check data validation
			if (string.IsNullOrEmpty(NameBox.Text) || string.IsNullOrEmpty(SurnameBox.Text))
			{
				MessageBox.Show("Imię ani nazwisko nie może być puste!", "Błąd imienia i/lub nazwiska", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (string.IsNullOrEmpty(EmailBox.Text) || !EmailBox.Text.Contains("@"))
			{
				MessageBox.Show("Nie podano prawidłowego adresu email!", "Błąd adresu email", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (string.IsNullOrEmpty(PasswordBox.Password))
			{
				MessageBox.Show("Nowe hasło nie może być puste!", "Błąd hasła", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var name = NameBox.Text;
				var surname = SurnameBox.Text;
				var email = EmailBox.Text;
				var newPassword = PasswordBox.Password;

				var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

				var connection = new SqlConnection(connString);

				var command = new SqlCommand($"SELECT * FROM BibliotekaDB.Czytelnicy WHERE Email={email}", connection);

				connection.Open();

				//?
				using (var reader = command.ExecuteReader())
				{
					reader.Read();

					if (!reader.HasRows)
					{
						MessageBox.Show("Podany użytkownik nie istnieje.", "Błąd użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
					}
					else
					{
						var dbName = reader["Name"].ToString();
						var dbSurname = reader["Surname"].ToString();

						if (dbName != name || dbSurname != surname)
						{
							MessageBox.Show("Niepoprawne dane użytkownika!", "Błąd użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
						}
						else
						{
							var updatePasswordCommand = new SqlCommand($"UPDATE BibliotekaDB.Czytelnicy SET Password='{newPassword}' WHERE Email={email}");
						}
					}
				}

				connection.Close();

			}

		}

		private void NoAccount_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var registerWindow = new RegisterWindow();
			registerWindow.Show();
			this.Close();
		}

		private void CloseButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
