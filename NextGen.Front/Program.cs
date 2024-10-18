using System.ComponentModel.Design;
using System.Reflection;
using System.Text.RegularExpressions;
using NextGen.Back.DependencyInjection;
using NextGen.Dal.Interfaces;
using NextGen.Back.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();



var builder = WebApplication.CreateBuilder(args);

string connectionString = configuration.GetConnectionString("database");
string pattern = @"Data Source=(.*?);";
string pathConnectionString = null;
if (Regex.Match(connectionString, pattern).Success)
    pathConnectionString = Regex.Match(connectionString, pattern).Groups[1].Value.Trim();

var pathFolder = Path.GetDirectoryName(pathConnectionString);
if (!Directory.Exists(pathFolder))
    Directory.CreateDirectory(pathFolder);

builder.Services.LoadServices(connectionString);

builder.Services.AddScoped<IActualiteSrv, ActualiteSrv>();
builder.Services.AddScoped<IUserSrv, UserSrv>();
builder.Services.AddScoped<ISourceSrv, SourceSrv>();
builder.Services.AddScoped<IQuestionSrv, QuestionSrv>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Rediriger vers la page de login si non connecté
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
