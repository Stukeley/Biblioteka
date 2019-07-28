using Biblioteka.Models;
using Biblioteka.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteka.UserControls
{
	/// <summary>
	/// Interaction logic for AccountPage.xaml
	/// </summary>
	public partial class AccountPage : UserControl
	{
		public AccountPage()
		{
			InitializeComponent();

			NameBox.Text = UserModel.CurrentUser.Imię;
			SurnameBox.Text = UserModel.CurrentUser.Nazwisko;
			EmailBox.Text = UserModel.CurrentUser.Email;
			PasswordBox.Text = UserModel.CurrentUser.Hasło;
			FeesBox.Text = UserModel.CurrentUser.Fees.ToString();

			if (!string.IsNullOrEmpty(Properties.Settings.Default.UserEmail) && !string.IsNullOrEmpty(Properties.Settings.Default.UserPassword))
			{
				RememberMeCheckBox.IsChecked = true;
			}
		}

		private void RememberMeCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.UserEmail = EmailBox.Text;
			Properties.Settings.Default.UserPassword = PasswordBox.Text;
		}

		private void RememberMeCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default.UserEmail = "";
			Properties.Settings.Default.UserPassword = "";
		}

		private void ChangeCredentialsButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new ChangeUserCredentialsWindow();
			window.Show();
			Application.Current.Windows[0].Close();
		}
	}
}
