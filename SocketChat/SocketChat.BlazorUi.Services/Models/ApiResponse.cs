namespace SocketChat.BlazorUi.Services.Models
{
	public class ApiResponse<T>
	{
		public T Data { get; set; }
		public List<string> ErrorMessages { get; set; }

		public int StatusCode { get; set; }


		public int DataCount { get; set; }
	}
}
