using Biblioteka.Controllers;
using Biblioteka.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for ReturnBookWindow.xaml
	/// </summary>
	public partial class ReturnBookWindow : Window
	{
		public BorrowingModel CurrentBorrowing;

		public ReturnBookWindow(BorrowingModel borrowing = null)
		{
			InitializeComponent();

			CurrentBorrowing = borrowing;

			if (CurrentBorrowing != null)
			{
				AuthorBox.Text = CurrentBorrowing.Autor.Imię + " " + CurrentBorrowing.Autor.Nazwisko;
				TitleBox.Text = CurrentBorrowing.TytułKsiążki;
				TimeLeftBox.Text = (CurrentBorrowing.TerminOddania - DateTime.Now).TotalDays.ToString();
			}
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentBorrowing != null)
			{
				LibraryDatabaseConnectionController.ReturnBook(CurrentBorrowing);
				this.Close();
			}
		}

		private void ExtendButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentBorrowing != null)
			{
				LibraryDatabaseConnectionController.ExtendUserBorrowing(CurrentBorrowing);
				this.Close();
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ExitButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void ExitButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
		}
	}
}
