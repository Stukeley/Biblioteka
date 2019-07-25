using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for BrowseUsers.xaml
	/// </summary>
	public partial class BrowseUsers : UserControl
	{
		private static List<UserModel> Użytkownicy;

		public BrowseUsers()
		{
			InitializeComponent();
			UpdateDataGrid();
		}

		private void UpdateDataGrid()
		{
			Użytkownicy = UserDatabaseConnectionController.GetAllUsers();
			WszyscyUżytkownicyDataGrid.ItemsSource = Użytkownicy;
			WszyscyUżytkownicyDataGrid.UpdateLayout();
		}
	}
}
