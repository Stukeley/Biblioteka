using Biblioteka.Controllers;
using Biblioteka.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for Homepage.xaml
	/// </summary>
	public partial class Homepage : UserControl
	{
		public Homepage()
		{
			InitializeComponent();

			if (UserModel.CurrentUser == null)
			{
				this.Visibility = Visibility.Hidden;
			}
			else
			{
				var amounts = DataInsertionController.GetAmountsOfItems();
				AmountOfAuthors.Text = amounts.Item2.ToString();
				AmountOfBorrowings.Text = LibraryDatabaseConnectionController.GetAllBorrowings().Where(x => x.UserId == UserModel.CurrentUser.Id)
					.ToList().Count().ToString();
				AmountOfBooks.Text = amounts.Item1.ToString();
				AmountOfAvailableBooks.Text = BookDatabaseConnectionController.GetAllBooks().Where(x => x.IsBorrowed == false)
					.ToList().Count().ToString();
			}
		}

		private void ContactUs_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as MainWindow;
			window.ChangeContent(new ContactPage());
		}

		private void AuthorsAmountRectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as MainWindow;
			window.ChangeContent(new AuthorsPage());
		}

		private void BorrowingsAmountRectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as MainWindow;
			window.ChangeContent(new BorrowingPage());
		}

		private void BooksAmountRectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var window = Window.GetWindow(this) as MainWindow;
			window.ChangeContent(new BooksPage());
		}

		private void AvailableBooksAmountRectangle_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			//filter books when this is clicked - to be done after the checkbox for showing unborrowed books only is added!
			var window = Window.GetWindow(this) as MainWindow;
			var booksPage = new BooksPage();
			booksPage.ShowAvailableBooksCheckbox.IsChecked = true;
			window.ChangeContent(booksPage);
		}
	}
}
