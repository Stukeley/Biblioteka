﻿using System;

namespace Biblioteka.Models
{
	public class AuthorModel
	{
		public int Id { get; set; }
		public string Imię { get; set; }
		public string Nazwisko { get; set; }
		public DateTime? DataUrodzenia { get; set; }
		public string Biografia { get; set; }

		//imię + nazwisko
		public string Nazwa => Imię + " " + Nazwisko;

		public AuthorModel()
		{
		}
	}
}
