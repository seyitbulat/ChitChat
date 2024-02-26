using SocketChat.BlazorUi.Services.Models;

namespace SocketChat.App.Session
{
	public class UserSession
	{
        public User User { get; set; }


		public void SetUser(User user)
		{
			User = user;
		}

		public User GetUser()
		{
			return User;
		}

	
		public void ClearUser()
		{
			User = null;

		}
    }
}
