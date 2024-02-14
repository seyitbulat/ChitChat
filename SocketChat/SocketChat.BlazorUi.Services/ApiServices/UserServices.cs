using SocketChat.BlazorUi.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SocketChat.BlazorUi.Services.ApiServices
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;

        public UserServices()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.19.10.42:3162");
        }


        public async Task<ApiResponse<List<User>>> UsersWithPage(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/Users/Page?PageNumber={pageNumber}&PageSize={pageSize}");
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var model = JsonSerializer.Deserialize<ApiResponse<List<Services.Models.User>>>(jsonResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

			var pagination = response.Headers.ToList().Where(e => e.Key == "X-Pagination").Select(e => e.Value).FirstOrDefault();
			var totalPagination = pagination.FirstOrDefault().Split(',').ToList()[1];

			int total = int.Parse(totalPagination.Split(":").ToList()[1]);
            model!.DataCount = total;
            return model;
		}


         public  async Task UsersWithPage(int pageNumber)
        {
			var response = await _httpClient.GetAsync($"/Users/Page?PageNumber={pageNumber}&PageSize=50");

		}
	}
}
