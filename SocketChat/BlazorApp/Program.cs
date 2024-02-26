using BlazorApp.ApiServices;
using BlazorApp.Data;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ApiService>();


builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();



app.MapBlazorHub();
app.MapFallbackToPage("/{*clientroutes:nonfile}", "/_Host").Add(b => ((RouteEndpointBuilder)b).Order = int.MaxValue - 1);
app.MapFallbackToPage("~/Admin/{*clientroutes:nonfile}", "/Admin/_HostAdmin").Add(b => ((RouteEndpointBuilder)b).Order = int.MaxValue - 1);

app.Run();
