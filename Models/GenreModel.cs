namespace Biblioteka.Models
{
	public class GenreModel
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
