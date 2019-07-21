using Biblioteka.Windows;
using System;

namespace Biblioteka.Exceptions
{
	internal class UserNotLoggedInException : Exception
	{
		public UserNotLoggedInException()
		{
			CustomMessageBox.Show("Użytkownik nie jest zalogowany!", "Aby uzyskać dostęp do tej strony, proszę się zalogować.",
				CustomMessageBox.CustomMessageBoxIcon.Warning);
		}

		public UserNotLoggedInException(string message) : base(message)
		{
		}

		public UserNotLoggedInException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
