namespace Biblioteka.Models
{
	internal class BookModel
	{
		//TODO: wydawnictwo?

		public int Id { get; set; }
		public string Tytuł { get; set; }
		public AuthorModel Autor { get; set; }
		public GenreModel Gatunek { get; set; }
		public bool IsBorrowed { get; set; }

		//bonus properties for DataGrids
		public string NazwaAutora { get; set; } //Imię + Nazwisko
		public string NazwaGatunku { get; set; }//Nazwa

		//might find a better way to handle these
		public int IdAutora { get; set; }
		public int IdGatunku { get; set; }

		public BookModel()
		{
		}

		public BookModel(string tytuł, AuthorModel autor = null, GenreModel gatunek = null)
		{
			Tytuł = tytuł;
			Autor = autor;
			Gatunek = gatunek;

			NazwaAutora = Autor.Imię + Autor.Nazwisko;
			NazwaGatunku = Gatunek.Nazwa;
		}
	}
}
