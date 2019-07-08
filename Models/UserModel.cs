namespace Biblioteka.Models
{
	internal class UserModel
	{
		//currently logged in user
		public static UserModel CurrentUser = null;

		//just for the currently logged in user, no other purposes
		public int UserId { get; set; }

		public string Imię { get; set; }
		public string Nazwisko { get; set; }
		public string Email { get; set; }
		public string Hasło { get; set; }

		private UserModel()
		{
		}

		public UserModel(string imię, string nazwisko, string email, string hasło)
		{
			Imię = imię;
			Nazwisko = nazwisko;
			Email = email;
			Hasło = hasło;
		}
	}
}
