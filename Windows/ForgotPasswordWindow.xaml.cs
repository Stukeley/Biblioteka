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
					var newPassword = PasswordBox.Password;

					UserDatabaseConnectionController.ResetUserPassword(name, surname, email, newPassword);
				}
				catch (UserNotFoundException)
				{
					UserNotFoundException.ShowGenericMessageBox();
				}
				catch (CredentialsException)
				{
					CredentialsException.ShowGenericMessageBox();
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

		private void TopRectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
