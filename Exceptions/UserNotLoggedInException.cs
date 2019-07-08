using System;

namespace Biblioteka.Exceptions
{
	internal class UserNotLoggedInException : Exception
	{
		public UserNotLoggedInException()
		{
		}

		public UserNotLoggedInException(string message) : base(message)
		{
		}

		public UserNotLoggedInException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
