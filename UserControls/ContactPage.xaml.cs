using Biblioteka.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for ContactPage.xaml
	/// </summary>
	public partial class ContactPage : UserControl
	{
		public ContactPage()
		{
			InitializeComponent();
		}

		private void SendButton_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(TitleBox.Text) && !string.IsNullOrWhiteSpace(TitleBox.Text) && !string.IsNullOrEmpty(ContentBox.Text) &&
				!string.IsNullOrWhiteSpace(ContentBox.Text))
			{
				CustomMessageBox.Show("Wysłano wiadomość!", "Wiadomość została poprawnie wysłana.", CustomMessageBox.CustomMessageBoxIcon.Information);

			}
			else
			{
				CustomMessageBox.Show("Błąd!", "Tytuł ani treść wiadomości nie mogą być puste", CustomMessageBox.CustomMessageBoxIcon.Warning);
			}
		}
	}
}
