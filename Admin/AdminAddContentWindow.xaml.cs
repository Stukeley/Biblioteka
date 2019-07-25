using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for AdminAddContentWindow.xaml
	/// </summary>
	public partial class AdminAddContentWindow : Window
	{
		public AdminAddContentWindow()
		{
			InitializeComponent();

			ChangeContent(new Dashboard());
		}

		public void ChangeContent(UserControl userControl)
		{
			if (!ContentGrid.Children.Contains(userControl))
			{
				ContentGrid.Children.Clear();
				ContentGrid.Children.Add(userControl);
			}
		}

		private void AddAuthor_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new AddNewAuthor());
		}

		private void AddGenre_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new AddNewGenre());
		}

		private void AddBook_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new AddNewBook());
		}

		private void BrowseUsers_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new BrowseUsers());
		}

		private void CloseWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}

		private void Dashboard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new Dashboard());
		}
	}
}
