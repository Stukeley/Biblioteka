using Biblioteka.Controllers;
using Biblioteka.Exceptions;
using Biblioteka.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for BorrowBookWindow.xaml
	/// </summary>
	public partial class BorrowBookWindow : Window
	{
		private BookModel CurrentBook;

		public BorrowBookWindow(BookModel book = null)
		{
			InitializeComponent();

			CurrentBook = book;

			if (CurrentBook != null)
			{
				AuthorBox.Text = CurrentBook.Autor.Imię + " " + CurrentBook.Autor.Nazwisko;
				TitleBox.Text = CurrentBook.Tytuł;
				DateBox.Text = DateTime.Now.AddDays(14).ToString();
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void ExitButton_MouseLeave(object sender, MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
		}

		private void BorrowButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentBook != null)
			{
				try
				{
					LibraryDatabaseConnectionController.BorrowBook(CurrentBook);
				}
				catch (UserNotLoggedInException)
				{
					UserNotLoggedInException.ShowGenericMessageBox();
				}
				catch (BookBorrowedException)
				{
					BookBorrowedException.ShowGenericMessageBox();
				}
				catch (TooManyBorrowings)
				{
					TooManyBorrowings.ShowGenericMessageBox();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}");
				}
			}
		}

		private void TopRectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
