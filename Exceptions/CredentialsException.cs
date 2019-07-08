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
	}
}
