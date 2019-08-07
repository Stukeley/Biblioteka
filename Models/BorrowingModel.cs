﻿using System;

namespace Biblioteka.Models
{
	public class BorrowingModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime DataWypożyczenia { get; set; }
		public DateTime TerminOddania { get; set; }

		public string NazwaAutora { get; set; }
		public string TytułKsiążki { get; set; }

		public AuthorModel Autor { get; set; }
		public BookModel Książka { get; set; }

		public BorrowingModel()
		{
		}
	}
}
