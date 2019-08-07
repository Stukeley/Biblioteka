using System;

namespace Biblioteka.Models
{
	public class UserModel
	{
		//currently logged in user
		public static UserModel CurrentUser = null;

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
			IsSpecialAccount = false;
		}
	}
}
