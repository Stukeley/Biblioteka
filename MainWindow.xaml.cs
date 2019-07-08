using System.Windows;
using System.Windows.Media;

namespace Biblioteka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//TODO: make the dashboard remind the user of logging in and make the menu unaccessible if not logged in
		//TODO: live chat in Contact section
		//TODO: (in the future) make tooltips
		//TODO: (in the future) machine learning for live chat

		public MainWindow()
		{
			InitializeComponent();

			//hide unnecessary elements if the user is logged in
			if (Biblioteka.Windows.LoginWindow.IsLoggedIn)
			{
				LoginButton.Visibility = Visibility.Hidden;
				PleaseLogInGrid.Visibility = Visibility.Hidden;
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

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void PleaseLogInGrid_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var loginWindow = new Biblioteka.Windows.LoginWindow();
			loginWindow.Show();
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
