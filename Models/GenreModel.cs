namespace Biblioteka.Models
{
	internal class GenreModel
	{
		public string Nazwa { get; set; }

		private GenreModel()
		{
		}

		public GenreModel(string nazwa)
		{
			Nazwa = nazwa;
		}
	}
}
