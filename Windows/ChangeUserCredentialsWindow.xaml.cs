using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using Biblioteka.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for ChangeUserCredentialsWindow.xaml
	/// </summary>
	public partial class ChangeUserCredentialsWindow : Window
	{
		public ChangeUserCredentialsWindow()
		{
			InitializeComponent();

			NameBox.Text = UserModel.CurrentUser.Imię;
			SurnameBox.Text = UserModel.CurrentUser.Nazwisko;
			EmailBox.Text = UserModel.CurrentUser.Email;
			PasswordBox.Password = UserModel.CurrentUser.Hasło;

		}

		private void ChangeCredentialsButton_Click(object sender, RoutedEventArgs e)
		{
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

					UserDatabaseConnectionController.UpdateUserCredentials(name, surname, email, password);
					CustomMessageBox.Show("Zmiana danych pomyślna!", "Pomyślnie dokonano zmiany danych użytkownika.",
						CustomMessageBox.CustomMessageBoxIcon.Information);

					var window = new MainWindow();
					window.ChangeContent(new AccountPage());
					window.Show();
					this.Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}");
				}
			}
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

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new MainWindow();
			window.ChangeContent(new AccountPage());
			window.Show();
			this.Close();
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
