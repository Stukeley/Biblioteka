namespace Biblioteka.Models
{
	internal class GenreModel
	{
		public int Id { get; set; }
		public string Nazwa { get; set; }

		public GenreModel()
		{
		}

		public GenreModel(string nazwa)
		{
			Nazwa = nazwa;
		}
	}
}
