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

namespace Biblioteka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//TODO: make the dashboard remind the user of logging in and make the menu unaccessible if not logged in

		public MainWindow()
		{
			InitializeComponent();

			//TODO: hide the button upon login
			if (Biblioteka.Windows.LoginWindow.IsLoggedIn)
			{
				LoginButton.Visibility = Visibility.Hidden;
			}
		}

		private void PopUpButtonLogout_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();

			//TODO: actually log out
		}

		private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonOpenMenu.Visibility = Visibility.Collapsed;
			ButtonCloseMenu.Visibility = Visibility.Visible;
		}

		private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
		{
			ButtonCloseMenu.Visibility = Visibility.Collapsed;
			ButtonOpenMenu.Visibility = Visibility.Visible;
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new Biblioteka.Windows.LoginWindow();
			loginWindow.Show();
			this.Close();
		}
	}
}
