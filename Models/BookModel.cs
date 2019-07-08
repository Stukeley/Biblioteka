namespace Biblioteka.Models
{
	internal class BookModel
	{
		//TODO: wydawnictwo?

		public string Tytuł { get; set; }
		public AuthorModel Autor { get; set; }
		public GenreModel Gatunek { get; set; }

		private BookModel()
		{
		}

		public BookModel(string tytuł, AuthorModel autor, GenreModel gatunek)
		{
			Tytuł = tytuł;
			Autor = autor;
			Gatunek = gatunek;
		}
	}
}
