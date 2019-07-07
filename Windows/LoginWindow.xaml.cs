using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		//TODO: add a checkbox to remember log-in credentials
		//TODO: hash passwords

		public static bool IsLoggedIn = false;

		public LoginWindow()
		{
			InitializeComponent();

			if (!string.IsNullOrEmpty(Properties.Settings.Default.UserEmail) && !string.IsNullOrEmpty(Properties.Settings.Default.UserPassword))
			{
				RememberLoginCredentials.IsChecked = true;
				EmailBox.Text = Properties.Settings.Default.UserEmail;
				PasswordBox.Password = Properties.Settings.Default.UserPassword;
			}
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			//Data validation
			if (string.IsNullOrEmpty(EmailBox.Text) || !EmailBox.Text.Contains("@"))
			{
				MessageBox.Show("Nie podano prawidłowego adresu email!", "Błąd adresu email", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (string.IsNullOrEmpty(PasswordBox.Password))
			{
				MessageBox.Show("Hasło nie może być puste!", "Błąd hasła", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var email = EmailBox.Text;
				var password = PasswordBox.Password;


				var connString = ConfigurationManager.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ToString();

				var connection = new SqlConnection(connString);

				var command = new SqlCommand($"SELECT * FROM BibliotekaDB.Czytelnicy WHERE Email='{email}'", connection);

				connection.Open();

				//?
				using (var reader = command.ExecuteReader())
				{
					reader.Read();

					if (!reader.HasRows)
					{
						MessageBox.Show("Podany adres email nie istnieje.", "Błąd adresu email", MessageBoxButton.OK, MessageBoxImage.Error);
					}
					else
					{
						var dbPassword = reader["Password"].ToString();

						if (dbPassword != password)
						{
							MessageBox.Show("Niepoprawne hasło!", "Błąd hasła", MessageBoxButton.OK, MessageBoxImage.Error);
							PasswordBox.Clear();
						}
						else
						{
							if (RememberLoginCredentials.IsChecked == true)
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

							IsLoggedIn = true;
						}
					}
				}

				connection.Close();
			}

			var mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}

		private void NoAccount_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var registerWindow = new RegisterWindow();
			registerWindow.Show();
			this.Close();
		}

		private void ForgotPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var forgotPasswordWindow = new ForgotPasswordWindow();
			forgotPasswordWindow.Show();
			this.Close();
		}

		private void CloseButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
