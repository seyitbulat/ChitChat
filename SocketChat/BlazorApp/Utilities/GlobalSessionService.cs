using BlazorApp.Model;

namespace BlazorApp.Utilities
{
	public static class GlobalSessionService
	{
		private static User _user;

        public static User Admin { get; set; }


        public static void SetLoggedUser(User user)
		{
			_user = user;
		}

		public static User GetLoggedUser()
		{
			return _user;
		}

		public static void ClearLoggedUser()
		{
			_user = null;
		}
    }
}
