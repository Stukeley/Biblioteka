using Biblioteka.Controllers;
using System.Windows.Controls;

namespace Biblioteka.Admin
{
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : UserControl
	{
		//TODO: change fonts

		public Dashboard()
		{
			InitializeComponent();

			UpdateAmounts();
		}

		private void UpdateAmounts()
		{
			var amounts = DataInsertionController.GetAmountsOfItems();

			AmountOfBooks.Text = amounts.Item1.ToString();
			AmountOfAuthors.Text = amounts.Item2.ToString();
			AmountOfGenres.Text = amounts.Item3.ToString();
			AmountOfUsers.Text = amounts.Item4.ToString();
		}
	}
}
