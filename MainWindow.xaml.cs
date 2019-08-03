using Biblioteka.Admin;
using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using Biblioteka.UserControls;
using Biblioteka.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Biblioteka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//TODO: live chat in Contact section
		//TODO: (in the future) make tooltips
		//TODO: (in the future) machine learning for live chat
		//TODO: add borrowing limit (every book can be borrowed by 1 person at a time, and every person can borrow up to 5 books at a time)
		//TODO: try-catches everywhere
		//TODO: get current directory at runtime and look for the database
		//might have trouble with inserting NULL into the database - to be checked


		public MainWindow()
		{
			InitializeComponent();

			//hide unnecessary elements if the user is logged in
			if (LoginWindow.IsLoggedIn)
			{
				LoginButton.Visibility = Visibility.Hidden;
				PleaseLogInGrid.Visibility = Visibility.Hidden;
				ChangeContent(new Homepage());

				UserDatabaseConnectionController.CountUserFees();

				//For simple adding of new books/authors - special privilege account/s. True means the account has special privileges
				if (UserModel.CurrentUser.IsSpecialAccount)
				{
					AdminAddContentButton.IsEnabled = true;
					AdminAddContentButton.Visibility = Visibility.Visible;
				}
			}
		}

		public void ChangeContent(UserControl userControl)
		{
			if (!ContentGrid.Children.Contains(userControl))
			{
				ContentGrid.Children.Clear();
				ContentGrid.Children.Add(userControl);
			}
		}

		private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonOpenMenu.Visibility = Visibility.Collapsed;
			ButtonCloseMenu.Visibility = Visibility.Visible;
		}

		private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonCloseMenu.Visibility = Visibility.Collapsed;
			ButtonOpenMenu.Visibility = Visibility.Visible;
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new Biblioteka.Windows.LoginWindow();
			loginWindow.Show();
			this.Close();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void PleaseLogInGrid_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var loginWindow = new Biblioteka.Windows.LoginWindow();
			loginWindow.Show();
			this.Close();
		}

		private void ExitButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void ExitButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
		}

		private void Homepage_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				ChangeContent(new Homepage());
			}
		}

		private void Books_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				ChangeContent(new BooksPage());
			}
		}

		private void Authors_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				ChangeContent(new AuthorsPage());
			}
		}

		private void Contact_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				ChangeContent(new ContactPage());
			}
		}

		private void Account_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				ChangeContent(new AccountPage());
			}
		}

		private void LogOut_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (UserModel.CurrentUser == null)
			{
				UserNotLoggedInException.ShowGenericMessageBox();
			}
			else
			{
				var loginWindow = new LoginWindow();
				loginWindow.Show();
				UserModel.CurrentUser = null;
				this.Close();
			}
		}

		private void AdminAddContentButton_Click(object sender, RoutedEventArgs e)
		{
			var addContentWindow = new AdminAddContentWindow();
			addContentWindow.Show();
			this.Close();
		}
	}
}
