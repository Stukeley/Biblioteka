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
	}
}
