using BlazorApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;

namespace BlazorApp.ApiServices
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResponse<T>> PostDataAsync<T, TPost>(string endPoint, TPost postObj, string token = null)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonObj = JsonSerializer.Serialize(postObj, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            HttpRequestMessage requestMessage;
            var uri = new Uri($"http://10.19.10.42:3162{endPoint}");

            requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://10.19.10.42:3162{endPoint}"),
                Headers = { { HeaderNames.Accept, "application/json" } },
                Content = new StringContent(jsonObj, System.Text.Encoding.UTF8, "application/json")
            };


            if (!string.IsNullOrEmpty(token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var responseMessage = await client.SendAsync(requestMessage);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                ApiResponse<T> response;

                response = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }

            return new ApiResponse<T> { ErrorMessages = new List<string> { responseMessage.ReasonPhrase}, StatusCode = 500 };
        }


        public async Task<ApiResponse<T>> GetDataAsync<T>(string endPoint, string token = null,Dictionary<string, string> queryStrings = null)
        {
            var client = _httpClientFactory.CreateClient();
            var uriBuilder = new UriBuilder($"http://10.19.10.42:3162{endPoint}");
            var query = HttpUtility.ParseQueryString(uriBuilder.Uri.Query);

            foreach (var qs in queryStrings)
            {
                query[qs.Key] = qs.Value;
             
            }

            uriBuilder.Query = query.ToString();
            var finalUri = uriBuilder.Uri;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var responseMessage = await client.GetAsync(finalUri);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                ApiResponse<T> response;

                response = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response;
            }
            return new ApiResponse<T> { ErrorMessages = new List<string> { responseMessage.ReasonPhrase }, StatusCode = 500 };
        }


        public async Task<HttpResponseMessage?> GetDataAsync(string endPoint, string token = null, Dictionary<string, string> queryStrings = null)
        {
            var client = _httpClientFactory.CreateClient();
            var uriBuilder = new UriBuilder($"http://10.19.10.42:3162{endPoint}");
            var query = HttpUtility.ParseQueryString(uriBuilder.Uri.Query);

            foreach (var qs in queryStrings)
            {
                query[qs.Key] = qs.Value;

            }

            uriBuilder.Query = query.ToString();
            var finalUri = uriBuilder.Uri;


            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var responseMessage = await client.GetAsync(finalUri);

            if (responseMessage.IsSuccessStatusCode)
            {
                
                
               
                return responseMessage;
            }

            return null;
        
        }
    }
}
