using System.Windows.Controls;
using System.Windows.Input;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for AuthorsPage.xaml
	/// </summary>
	public partial class AuthorsPage : UserControl
	{
		public AuthorsPage()
		{
			InitializeComponent();
		}

		private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//pokaż biografię
		}
	}
}
