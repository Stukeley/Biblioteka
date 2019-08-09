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

		private void ShowHidePassword_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (ShowHidePassword.Kind == MaterialDesignThemes.Wpf.PackIconKind.Eye)
			{
				ShowHidePassword.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;

				var enteredPassword = PasswordBox.Password;

				VisiblePasswordBox.Visibility = Visibility.Visible;
				VisiblePasswordBox.Text = enteredPassword;
				PasswordBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				ShowHidePassword.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;

				var enteredPassword = VisiblePasswordBox.Text;

				PasswordBox.Visibility = Visibility.Visible;
				PasswordBox.Password = enteredPassword;
				VisiblePasswordBox.Visibility = Visibility.Collapsed;
			}
		}

		private void TopRectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
