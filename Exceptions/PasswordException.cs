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
	}
}
