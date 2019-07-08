using System;

namespace Biblioteka.Exceptions
{
	internal class NoBorrowingFoundException : Exception
	{
		public NoBorrowingFoundException()
		{
		}

		public NoBorrowingFoundException(string message) : base(message)
		{
		}

		public NoBorrowingFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
