namespace Biblioteka.Models
{
	public class BookModel
	{
		public int Id { get; set; }
		public string Tytuł { get; set; }
		public AuthorModel Autor { get; set; }
		public GenreModel Gatunek { get; set; }
		public bool IsBorrowed { get; set; }

		//bonus properties for DataGrids
		public string NazwaAutora { get; set; } //Imię + Nazwisko
		public string NazwaGatunku { get; set; }//Nazwa

		public BookModel()
		{
		}
	}
}
