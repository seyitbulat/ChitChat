using Microsoft.FluentUI.AspNetCore.Components;
using SocketChat.BlazorUi.Client.Pages;
using SocketChat.BlazorUi.Components;
using SocketChat.BlazorUi.Services.ApiServices;

namespace SocketChat.BlazorUi
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient();

			builder.Services.AddScoped<UserServices>();

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://10.19.10.42:3162") });

			builder.Services.AddFluentUIComponents();



			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents()
				.AddInteractiveWebAssemblyComponents();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode()
				.AddInteractiveWebAssemblyRenderMode()
				.AddAdditionalAssemblies(typeof(Counter).Assembly);

			app.Run();
		}
	}
}
