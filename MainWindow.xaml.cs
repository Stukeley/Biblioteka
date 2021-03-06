﻿using Biblioteka.Admin;
using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using Biblioteka.UserControls;
using Biblioteka.Windows;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Biblioteka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//! Current BibliotekaDBConnectionString: 
		//? Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Programowanie\Biblioteka\BibliotekaDB.mdf;Integrated Security=True;Connect Timeout=30

		private static string ConnectionStringBase = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;" +
			"AttachDbFilename=" + Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\BibliotekaDB.mdf;";

		static MainWindow()
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.ConnectionStrings.ConnectionStrings["Biblioteka.Properties.Settings.BibliotekaDBConnectionString"].ConnectionString = ConnectionStringBase;
			config.Save();
			ConfigurationManager.RefreshSection("connectionStrings");
		}

		public MainWindow()
		{
			InitializeComponent();

			//hide unnecessary elements if the user is logged in
			if (UserModel.CurrentUser != null)
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
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			this.Close();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void PleaseLogInGrid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
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

		private void Homepage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void Books_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void Authors_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void Contact_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void Account_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void LogOut_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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

		private void DragableTop_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
