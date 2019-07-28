using System;

namespace Biblioteka.Models
{
	public class AuthorModel
	{
		public int Id { get; set; }
		public string Imię { get; set; }
		public string Nazwisko { get; set; }
		public DateTime? DataUrodzenia { get; set; }
		public string Biografia { get; set; }

		public AuthorModel()
		{
		}

		public AuthorModel(string imię, string nazwisko, DateTime? dataUrodzenia = null, string biografia = null)
		{
			Imię = imię;
			Nazwisko = nazwisko;
			DataUrodzenia = dataUrodzenia;
			Biografia = biografia;
		}
	}
}
