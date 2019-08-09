using Biblioteka.Controllers;
using Biblioteka.Models;
using Biblioteka.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for BorrowingPage.xaml
	/// </summary>
	public partial class BorrowingPage : UserControl
	{
		public static List<BorrowingModel> Wypożyczenia;

		public BorrowingPage()
		{
			InitializeComponent();

			UpdateDataGrid();
		}

		public void UpdateDataGrid()
		{
			Wypożyczenia = LibraryDatabaseConnectionController.RetrieveBorrowingsForUser();
			WszystkieWypożyczeniaDataGrid.ItemsSource = Wypożyczenia;
			WszystkieWypożyczeniaDataGrid.UpdateLayout();

			FeesBox.Text = FeesBox.Text + UserModel.CurrentUser.Fees.ToString() + "zł";
		}

		private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var row = sender as DataGridRow;
			var borrowing = row.Item as BorrowingModel;
			var returnWindow = new ReturnBookWindow()
			{
				CurrentBorrowing = borrowing
			};
			returnWindow.Show();
		}
	}
}
