using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
				try
				{
					var name = NameBox.Text;
					var surname = SurnameBox.Text;
					var email = EmailBox.Text;
					var newPassword = PasswordBox.Password;

					UserDatabaseConnectionController.ResetUserPassword(name, surname, email, newPassword);
				}
				catch (UserNotFoundException)
				{
					MessageBox.Show("Podany użytkownik nie istnieje.", "Błąd użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (CredentialsException)
				{
					MessageBox.Show("Niepoprawne dane użytkownika!", "Błąd użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}");
				}
			}

		}

		private void NoAccount_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var registerWindow = new RegisterWindow();
			registerWindow.Show();
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
