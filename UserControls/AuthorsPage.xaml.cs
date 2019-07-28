using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for AuthorsPage.xaml
	/// </summary>
	public partial class AuthorsPage : UserControl
	{
		private static List<AuthorModel> Autorzy;
		private static List<AuthorModel> NowiAutorzy;

		public AuthorsPage()
		{
			InitializeComponent();

			UpdateDataGrids();
		}

		private void UpdateDataGrids()
		{
			Autorzy = AuthorsDatabaseConnectionController.GetAllAuthors();
			WszyscyAutorzyDataGrid.ItemsSource = Autorzy;
			WszyscyAutorzyDataGrid.UpdateLayout();

			NowiAutorzy = AuthorsDatabaseConnectionController.GetRecentAuthors();
			NowiAutorzyDataGrid.ItemsSource = NowiAutorzy;
			NowiAutorzyDataGrid.UpdateLayout();
		}

		private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var row = sender as DataGridRow;
			var authorId = (row.Item as AuthorModel).Id;
			var nameAndSurname = (row.Item as AuthorModel).Imię + (row.Item as AuthorModel).Nazwisko;

			MessageBox.Show(AuthorsDatabaseConnectionController.GetAuthorBiography(authorId), nameAndSurname);
		}

		private void FilterAuthorsButton_Click(object sender, RoutedEventArgs e)
		{
			string name = null, surname = null;

			if (!string.IsNullOrEmpty(NameBox.Text) && !string.IsNullOrWhiteSpace(NameBox.Text))
			{
				name = NameBox.Text;
			}

			if (!string.IsNullOrEmpty(SurnameBox.Text) && !string.IsNullOrWhiteSpace(SurnameBox.Text))
			{
				surname = SurnameBox.Text;
			}

			var filteredAuthors = AuthorsDatabaseConnectionController.FilterAuthors(name, surname);

			WszyscyAutorzyDataGrid.ItemsSource = filteredAuthors;
		}

		private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
		{
			UpdateDataGrids();
		}

		private void WszyscyAutorzyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var row = sender as DataGridRow;
			var authorId = (row.Item as AuthorModel).Id;
			var nameAndSurname = (row.Item as AuthorModel).Imię + (row.Item as AuthorModel).Nazwisko;

			MessageBox.Show(AuthorsDatabaseConnectionController.GetAuthorBiography(authorId), nameAndSurname);
		}
	}
}
