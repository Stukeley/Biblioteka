using System.Windows;
using System.Windows.Input;

namespace Biblioteka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//TODO: make the dashboard remind the user of logging in and make the menu unaccessible if not logged in
		//TODO: move the menu from top right to left side menu, make an X in its place

		public MainWindow()
		{
			InitializeComponent();

			//TODO: hide the button upon login
			if (Biblioteka.Windows.LoginWindow.IsLoggedIn)
			{
				LoginButton.Visibility = Visibility.Hidden;
			}
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

		private void ExitButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
