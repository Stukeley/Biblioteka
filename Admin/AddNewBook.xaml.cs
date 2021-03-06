﻿using Biblioteka.Controllers;
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
			var autorzyString = new List<string>();
			foreach (var autor in Autorzy)
			{
				autorzyString.Add(autor.Imię + " " + autor.Nazwisko);
			}
			AuthorNameBox.ItemsSource = autorzyString;

			Gatunki = GenresDatabaseConnectionController.GetAllGenres();
			var gatunkiString = new List<string>();
			foreach (var gatunek in Gatunki)
			{
				gatunkiString.Add(gatunek.Nazwa);
			}
			BookGenreBox.ItemsSource = gatunkiString;

			foreach (var autor in Autorzy)
			{
				authorNamesAndIds.Add(autor.Id, autor.Imię + " " + autor.Nazwisko);
			}

			foreach (var gatunek in Gatunki)
			{
				genreNamesAndIds.Add(gatunek.Id, gatunek.Nazwa);
			}

			Książki = BookDatabaseConnectionController.GetAllBooks();

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
			if (string.IsNullOrEmpty(BookTitleBox.Text) || string.IsNullOrWhiteSpace(BookTitleBox.Text) || string.IsNullOrEmpty(BookGenreBox.Text)
				|| string.IsNullOrWhiteSpace(BookGenreBox.Text) || string.IsNullOrEmpty(AuthorNameBox.Text) || string.IsNullOrWhiteSpace(AuthorNameBox.Text))
			{
				MessageBox.Show("Puste pola!", "Błąd przy dodawaniu do bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var title = BookTitleBox.Text;
				//imię + nazwisko
				var authorName = AuthorNameBox.Text;
				var genreName = BookGenreBox.Text;

				var author = Autorzy.First(x => x.Nazwa == authorName);

				var książka = new BookModel()
				{
					Tytuł = title,
					Autor = author,
					Gatunek = GenresDatabaseConnectionController.GetGenreByName(genreName),
					IsBorrowed = false,
				};
				DataInsertionController.InsertBookIntoDatabase(książka);
				UpdateDataGrid();

				AuthorNameBox.Text = "";
				BookTitleBox.Text = "";
				BookGenreBox.Text = "";
			}
		}
	}
}
