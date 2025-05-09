using HackerNews_Assessment.Repositories;
using HackerNews_Assessment.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<HNMemoryCache>();
builder.Services.AddScoped<IHackerNewsRepository,HackerNewsRepository>();
builder.Services.AddScoped<IHackerNewsService,HackerNewsService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllers();

app.MapFallbackToFile("index.html");;

app.Run();
