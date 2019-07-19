using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for Homepage.xaml
	/// </summary>
	public partial class Homepage : UserControl
	{
		public Homepage()
		{
			InitializeComponent();

			if (UserModel.CurrentUser == null)
			{
				this.Visibility = Visibility.Hidden;
			}
			else
			{
				WypożyczeniaDataGrid.ItemsSource = LibraryDatabaseConnectionController.RetrieveBorrowingsForUser();
			}

			WypożyczeniaDataGrid.ItemsSource = LibraryDatabaseConnectionController.RetrieveBorrowingsForUser();
			OstatnieWypożyczenieDataGrid.ItemsSource = new List<BorrowingModel>()
			{
				LibraryDatabaseConnectionController.RetrieveRecentBorrowing()
			};

			KoniecTerminuDataGrid.ItemsSource = LibraryDatabaseConnectionController.RetrieveEndingBorrowings();
		}

		private void ContactUs_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
