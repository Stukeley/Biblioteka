using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class BookBorrowedException : Exception
	{
		public BookBorrowedException() : base()
		{
		}

		public BookBorrowedException(string message) : base(message)
		{
		}

		public BookBorrowedException(string message, Exception inner) : base(message, inner)
		{
		}

		public static void ShowGenericMessageBox()
		{
			CustomMessageBox.Show("Błąd wypożyczenia!", "Ta pozycja jest obecnie wypożyczona.", CustomMessageBox.CustomMessageBoxIcon.Warning);
		}
	}
}
