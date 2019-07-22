using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class UserNotFoundException : Exception
	{
		public UserNotFoundException()
		{
		}

		public UserNotFoundException(string message) : base(message)
		{
		}

		public UserNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Nie znaleziono użytkownika!", "Nie znaleziono użytkownika w bazie danych.",
				CustomMessageBox.CustomMessageBoxIcon.Error);
		}
	}
}
