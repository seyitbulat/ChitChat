using SocketChat.MvcUi.Models;

namespace SocketChat.MvcUi.GlobalStroge
{
	public static class ActiveUser
	{
        public static UserModel User{ get; set; } = new UserModel();
    }
}
