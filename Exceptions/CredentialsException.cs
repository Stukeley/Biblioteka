using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class CredentialsException : Exception
	{
		public CredentialsException()
		{
		}

		public CredentialsException(string message) : base(message)
		{
		}

		public CredentialsException(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Błąd użytkownika!", "Niepoprawne dane użytkownika.", CustomMessageBox.CustomMessageBoxIcon.Error);
		}

		public static void IncorrectNameSurnameFormat()
		{
			CustomMessageBox.Show("Błąd użytkownika!", "Imię ani nazwisko nie może być puste", CustomMessageBox.CustomMessageBoxIcon.Error);
		}
	}
}
