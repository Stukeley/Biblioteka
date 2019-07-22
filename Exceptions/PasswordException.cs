using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class PasswordException : Exception
	{
		public PasswordException()
		{
		}

		public PasswordException(string message) : base(message)
		{
		}

		public PasswordException(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Niepoprawne hasło!", "Wprowadzono niepoprawne hasło dla podanego adresu email.",
				CustomMessageBox.CustomMessageBoxIcon.Error);
		}
	}
}
