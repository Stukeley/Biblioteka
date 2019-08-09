using Biblioteka.Controllers;
using System;

namespace Biblioteka.Models
{
	public class BorrowingModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime DataWypożyczenia { get; set; }
		public DateTime TerminOddania { get; set; }

		public BookModel Książka => BookDatabaseConnectionController.GetBookById(BookId);

		public string NazwaAutora => Książka.Autor.Nazwa;
		public string TytułKsiążki => Książka.Tytuł;

		public BorrowingModel()
		{
		}
	}
}
