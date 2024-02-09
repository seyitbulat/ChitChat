using SocketChat.MvcUi.Models;

namespace SocketChat.MvcUi.GlobalStroge
{
	public static class ActiveUsers
	{
        
        public static List<UserModel> Users { get; set; }=new List<UserModel>();
    }
}
