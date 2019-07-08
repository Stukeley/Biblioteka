using System;

namespace Biblioteka.Models
{
	internal class AuthorModel
	{
		public string Imię { get; set; }
		public string Nazwisko { get; set; }
		public DateTime? DataUrodzenia { get; set; }

		private AuthorModel()
		{
		}

		public AuthorModel(string imię, string nazwisko)
		{
			Imię = imię;
			Nazwisko = nazwisko;
			DataUrodzenia = null;
		}

		public AuthorModel(string imię, string nazwisko, DateTime? dataUrodzenia)
		{
			Imię = imię;
			Nazwisko = nazwisko;
			DataUrodzenia = dataUrodzenia;
		}
	}
}
