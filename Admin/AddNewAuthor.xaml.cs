using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for AddNewAuthor.xaml
	/// </summary>
	public partial class AddNewAuthor : UserControl
	{
		//TODO: event on update or deletion of a row (commit this to the DB)

		//Lista ze wszystkimi autorami
		private static List<AuthorModel> Autorzy;

		public AddNewAuthor()
		{
			InitializeComponent();

			//foreach (var author in autorzy)
			//{
			//	WszyscyAutorzyDataGrid.Items.Add(author);
			//}

			UpdateDataGrid();
		}

		private void UpdateDataGrid()
		{
			Autorzy = AuthorsDatabaseConnectionController.GetAllAuthors();
			WszyscyAutorzyDataGrid.ItemsSource = Autorzy;
			WszyscyAutorzyDataGrid.UpdateLayout();
		}

		private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(AuthorNameBox.Text) || string.IsNullOrEmpty(AuthorSurnameBox.Text) || string.IsNullOrWhiteSpace(AuthorNameBox.Text)
				|| string.IsNullOrWhiteSpace(AuthorSurnameBox.Text))
			{
				MessageBox.Show("Puste pola!", "Błąd przy dodawaniu do bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var name = AuthorNameBox.Text;
				var surname = AuthorSurnameBox.Text;
				var dateOfBirth = AuthorDateOfBirthPicker.SelectedDate;
				var biography = AuthorBiographyBox.Text;

				var author = new AuthorModel()
				{
					Imię = name,
					Nazwisko = surname,
					DataUrodzenia = dateOfBirth,
					Biografia = biography
				};

				DataInsertionController.InsertAuthorIntoDatabase(author);
				//WszyscyAutorzyDataGrid.Items.Add(author);
				UpdateDataGrid();
				AuthorNameBox.Text = "";
				AuthorSurnameBox.Text = "";
				AuthorBiographyBox.Text = "";
			}
		}

		private void WszyscyAutorzyDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var row = sender as DataGridRow;
			var authorId = (row.Item as AuthorModel).Id;
			var nameAndSurname = (row.Item as AuthorModel).Imię + " " + (row.Item as AuthorModel).Nazwisko;

			MessageBox.Show(AuthorsDatabaseConnectionController.GetAuthorBiography(authorId), nameAndSurname);
		}

		private void WszyscyAutorzyDataGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Delete)
			{
				//?
				var selection = ((sender as DataGrid).SelectedItem) as AuthorModel;
				Autorzy.Remove(selection);
				UpdateDataGrid();
			}
		}

		private void WszyscyAutorzyDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			//?
		}
	}
}
