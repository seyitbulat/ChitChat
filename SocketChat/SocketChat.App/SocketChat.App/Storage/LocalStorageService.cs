using Microsoft.JSInterop;

namespace SocketChat.App.Storage
{
	public class LocalStorageService
	{
		private IJSRuntime jSRuntime;

		public LocalStorageService(IJSRuntime jSRuntime)
		{
			this.jSRuntime = jSRuntime;
		}

		public async Task AddItem(string key, string value)
		{
			await jSRuntime.InvokeVoidAsync("localStorage.addItem", key, value);
		}

		public async Task<string> GetItem(string key)
		{
			return await jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
		}
	}
}
