using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for AddNewBook.xaml
	/// </summary>
	public partial class AddNewBook : UserControl
	{
		private static List<AuthorModel> Autorzy;
		private static List<GenreModel> Gatunki;

		private static List<BookModel> Książki;

		public AddNewBook()
		{
			InitializeComponent();

			UpdateDataGrid();
		}

		private void UpdateDataGrid()
		{
			var authorNamesAndIds = new Dictionary<int, string>();
			var genreNamesAndIds = new Dictionary<int, string>();

			Autorzy = AuthorsDatabaseConnectionController.GetAllAuthors();
			Gatunki = GenresDatabaseConnectionController.GetAllGenres();

			foreach (var autor in Autorzy)
			{
				authorNamesAndIds.Add(autor.Id, autor.Imię + autor.Nazwisko);
			}

			foreach (var gatunek in Gatunki)
			{
				genreNamesAndIds.Add(gatunek.Id, gatunek.Nazwa);
			}

			//check the method
			Książki = BookDatabaseConnectionController.GetAllBooks();

			for (int i = 0; i < Książki.Count; i++)
			{
				Książki[i].NazwaAutora = authorNamesAndIds.First(x => x.Key == Książki[i].IdAutora).Value;
				Książki[i].NazwaGatunku = genreNamesAndIds.First(x => x.Key == Książki[i].IdGatunku).Value;
			}

			WszystkieKsiążkiDataGrid.ItemsSource = Książki;
			WszystkieKsiążkiDataGrid.UpdateLayout();
		}

		private void WszystkieKsiążkiDataGrid_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void WszystkieKsiążkiDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{

		}

		private void AddBookButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
