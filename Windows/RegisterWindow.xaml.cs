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
	/// Interaction logic for RegisterWindow.xaml
	/// </summary>
	public partial class RegisterWindow : Window
	{
		//TODO: What if the Email already exists in the database? To be checked
		public RegisterWindow()
		{
			InitializeComponent();
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			//check data validation

			if (string.IsNullOrEmpty(NameBox.Text) || string.IsNullOrEmpty(SurnameBox.Text))
			{
				CredentialsException.IncorrectNameSurnameFormat();
			}
			else if (string.IsNullOrEmpty(EmailBox.Text) || !EmailBox.Text.Contains("@"))
			{
				EmailException.IncorrectEmailFormat();
			}
			else if (string.IsNullOrEmpty(PasswordBox.Password) || PasswordBox.Password.Length > 30)
			{
				PasswordException.IncorrectPasswordFormat();
			}
			else
			{
				try
				{
					var name = NameBox.Text;
					var surname = SurnameBox.Text;
					var email = EmailBox.Text;
					var password = PasswordBox.Password;

					UserDatabaseConnectionController.RegisterUser(name, surname, email, password);
					CustomMessageBox.Show("Rejestracja pomyślna!", "Udało się zarejestrować pomyślnie. Możesz się teraz zalogować",
						CustomMessageBox.CustomMessageBoxIcon.Information);

					var loginWindow = new LoginWindow();
					loginWindow.Show();
					this.Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}");
				}
			}
		}

		private void AlreadyHaveAnAccount_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
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
