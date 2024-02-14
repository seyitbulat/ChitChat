using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://10.19.10.42:3162") });


await builder.Build().RunAsync();
