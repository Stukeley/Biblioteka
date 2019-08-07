using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for AddNewGenre.xaml
	/// </summary>
	public partial class AddNewGenre : UserControl
	{
		private List<GenreModel> Gatunki;

		public AddNewGenre()
		{
			InitializeComponent();

			UpdateDataGrid();
		}

		private void UpdateDataGrid()
		{
			Gatunki = GenresDatabaseConnectionController.GetAllGenres();
			WszystkieGatunkiDataGrid.ItemsSource = Gatunki;
			WszystkieGatunkiDataGrid.UpdateLayout();
		}

		private void WszystkieGatunkiDataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			//TO BE TESTED ON AddNewAuthor
		}

		private void WszystkieGatunkiDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{

		}

		private void AddGenreButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(GenreNameBox.Text) || string.IsNullOrWhiteSpace(GenreNameBox.Text))
			{
				MessageBox.Show("Puste pola!", "Błąd przy dodawaniu do bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var name = GenreNameBox.Text;

				var gatunek = new GenreModel()
				{
					Nazwa = name
				};

				DataInsertionController.InsertGenreIntoDatabase(gatunek);
				UpdateDataGrid();
				GenreNameBox.Text = "";
			}
		}
	}
}
