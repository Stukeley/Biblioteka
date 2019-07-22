using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class TooManyBorrowings : Exception
	{
		public TooManyBorrowings() : base()
		{
		}

		public TooManyBorrowings(string message) : base(message)
		{
		}

		public TooManyBorrowings(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Osiągnieto limit wypożyczeń!", "Maksymalna liczba wypożyczeń dla zwykłego użytkownika wynosi 5.",
				CustomMessageBox.CustomMessageBoxIcon.Error);
		}
	}
}
