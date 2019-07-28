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

		private void FilterBooksButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string author = null, title = null;

			if (!string.IsNullOrEmpty(AuthorBox.Text) && !string.IsNullOrWhiteSpace(AuthorBox.Text))
			{
				author = AuthorBox.Text;
			}

			if (!string.IsNullOrEmpty(TitleBox.Text) && !string.IsNullOrWhiteSpace(TitleBox.Text))
			{
				title = TitleBox.Text;
			}

			var filteredBooks = BookDatabaseConnectionController.FilterBooks(author, title);

			WszystkieKsiążkiDataGrid.ItemsSource = filteredBooks;
		}

		private void ResetFiltersButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			UpdateDataGrids();
		}
	}
}
