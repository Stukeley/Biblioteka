using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Biblioteka.Models;
using Biblioteka.Controllers;

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

			if (UserModel.CurrentUser==null)
			{
				this.Visibility = Visibility.Hidden;
			}
			else
			{
				WypożyczeniaDataGrid.ItemsSource = LibraryDatabaseConnectionController.RetrieveBorrowingsForUser();
			}
		}

		private void ContactUs_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
