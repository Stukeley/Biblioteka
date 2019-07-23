using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class EmailException : Exception
	{
		public EmailException()
		{
		}

		public EmailException(string message) : base(message)
		{
		}

		public EmailException(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Błąd adresu email", "Podany adres email nie istnieje w bazie danych!", CustomMessageBox.CustomMessageBoxIcon.Error);
		}

		public static void IncorrectEmailFormat()
		{
			CustomMessageBox.Show("Błąd adresu email", "Nie podano prawidłowego adresu email.", CustomMessageBox.CustomMessageBoxIcon.Error);
		}
	}
}
