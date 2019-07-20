namespace Biblioteka.Models
{
	internal class BookModel
	{
		//TODO: wydawnictwo?

		public int Id { get; set; }
		public string Tytuł { get; set; }
		public AuthorModel Autor { get; set; }
		public GenreModel Gatunek { get; set; }

		public BookModel()
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
