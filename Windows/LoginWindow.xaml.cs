using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
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
				try
				{
					var email = EmailBox.Text;
					var password = PasswordBox.Password;

					UserDatabaseConnectionController.GetUserByEmail(email, password, RememberLoginCredentials.IsChecked);

					CustomMessageBox.Show("Zalogowano pomyślnie!", "Zalogowano pomyślnie, możesz teraz przejść do aplikacji.",
						CustomMessageBox.CustomMessageBoxIcon.Information);
				}
				catch (EmailException)
				{
					EmailException.ShowGenericMessageBox();
				}
				catch (PasswordException)
				{
					PasswordException.ShowGenericMessageBox();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}");
				}
			}

			if (UserModel.CurrentUser != null)
			{
				var mainWindow = new MainWindow();
				mainWindow.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("Nie udało się uzyskać użytkownika z bazy danych!", "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
			}
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

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
		}
	}
}
