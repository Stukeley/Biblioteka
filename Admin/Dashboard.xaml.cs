using Biblioteka.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : UserControl
	{
		//TODO: change fonts

		public Dashboard()
		{
			InitializeComponent();

			UpdateAmounts();
		}

		private void UpdateAmounts()
		{
			var amounts = DataInsertionController.GetAmountsOfItems();

			AmountOfBooks.Text = amounts.Item1.ToString();
			AmountOfAuthors.Text = amounts.Item2.ToString();
			AmountOfGenres.Text = amounts.Item3.ToString();
			AmountOfUsers.Text = amounts.Item4.ToString();
		}

		private void BooksAmountRectangle_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as AdminAddContentWindow;
			window.ChangeContent(new AddNewBook());
		}

		private void AuthorsAmountRectangle_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as AdminAddContentWindow;
			window.ChangeContent(new AddNewAuthor());
		}

		private void GenresAmountRectangle_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as AdminAddContentWindow;
			window.ChangeContent(new AddNewGenre());
		}

		private void UsersAmountRectangle_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as AdminAddContentWindow;
			window.ChangeContent(new BrowseUsers());
		}
	}
}
