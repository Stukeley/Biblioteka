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

		private void ChangeContent(UserControl userControl)
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

		}

		private void AddBook_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void BrowseUsers_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{

		}

		private void CloseWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}
	}
}
