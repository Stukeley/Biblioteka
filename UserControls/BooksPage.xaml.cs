using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for BooksPage.xaml
	/// </summary>
	public partial class BooksPage : UserControl
	{
		private static List<BookModel> Książki;

		private static List<BookModel> NoweKsiążki;

		public BooksPage()
		{
			InitializeComponent();
			UpdateDataGrids();
		}

		private void UpdateDataGrids()
		{
			Książki = BookDatabaseConnectionController.GetUnborrowedBooks();
			WszystkieKsiążkiDataGrid.ItemsSource = Książki;
			WszystkieKsiążkiDataGrid.UpdateLayout();

			NoweKsiążki = BookDatabaseConnectionController.GetRecentBooks();
			NoweKsiążkiDataGrid.ItemsSource = NoweKsiążki;
			NoweKsiążkiDataGrid.UpdateLayout();
		}
	}
}
