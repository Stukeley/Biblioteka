using Biblioteka.Controllers;
using Biblioteka.Models;
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
		//TODO: show author's biography when row is doubleclicked

		public AddNewAuthor()
		{
			InitializeComponent();

			var authors = AuthorsDatabaseConnectionController.GetAllAuthors();
			foreach (var author in authors)
			{
				WszyscyAutorzyDataGrid.Items.Add(author);
			}
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

				var author = new AuthorModel(name, surname, dateOfBirth, biography);

				DataInsertionController.InsertAuthorIntoDatabase(author);
				WszyscyAutorzyDataGrid.Items.Add(author);

			}
		}
	}
}
