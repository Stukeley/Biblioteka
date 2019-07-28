using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using Biblioteka.Windows;
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

		private void ShowAvailableBooksCheckbox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			var availableBooks = BookDatabaseConnectionController.GetUnborrowedBooks();
			WszystkieKsiążkiDataGrid.ItemsSource = availableBooks;
			WszystkieKsiążkiDataGrid.UpdateLayout();
		}

		private void ShowAvailableBooksCheckbox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
			UpdateDataGrids();
		}

		private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var row = sender as DataGridRow;
			var book = row.Item as BookModel;

			if (!book.IsBorrowed)
			{
				var borrowWindow = new BorrowBookWindow(book);
				borrowWindow.Show();
			}
			else
			{
				BookBorrowedException.ShowGenericMessageBox();
			}
		}
	}
}
