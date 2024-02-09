namespace SocketChat.MvcUi.Models
{
	public  class Users
	{
		public List<UserModel> userList ;

		public Users()
		{
			this.userList = new List<UserModel>();
		}


		public void AddUser(UserModel user)
		{
			userList.Add(user);
		}
	
	}
}
