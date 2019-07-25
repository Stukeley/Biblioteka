using System;

namespace Biblioteka.Models
{
	internal class UserModel
	{
		//currently logged in user
		public static UserModel CurrentUser = null;

		//just for the currently logged in user, no other purposes
		public int UserId { get; set; }

		public int Id { get; set; }
		public string Imię { get; set; }
		public string Nazwisko { get; set; }
		public string Email { get; set; }
		public string Hasło { get; set; }
		public DateTime? DateOfCreation { get; set; }
		public bool IsSpecialAccount { get; set; }
		public int Fees { get; set; }

		public UserModel()
		{
		}

		public UserModel(string imię, string nazwisko, string email, string hasło, DateTime? dateOfCreation = null)
		{
			Imię = imię;
			Nazwisko = nazwisko;
			Email = email;
			Hasło = hasło;
			DateOfCreation = dateOfCreation;
			IsSpecialAccount = false;
		}
	}
}
