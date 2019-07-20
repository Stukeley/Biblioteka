﻿using System;

namespace Biblioteka.Models
{
	internal class BorrowingModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime DataWypożyczenia { get; set; }
		public DateTime TerminOddania { get; set; }

		public string NazwaAutora { get; set; }
		public string TytułKsiążki { get; set; }

		public BorrowingModel()
		{
		}

		public BorrowingModel(int userId, int bookId, DateTime dataWypożyczenia, DateTime terminOddania)
		{
			UserId = userId;
			BookId = bookId;
			DataWypożyczenia = dataWypożyczenia;
			TerminOddania = terminOddania;
		}
	}
}
