using System.ComponentModel.Design;
using System.Reflection;
using System.Text.RegularExpressions;
using NextGen.Back.DependencyInjection;
using NextGen.Dal.Interfaces;
using NextGen.Back.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();


// Récupérer la section EMailSettings et la mettre dans les claims de User
var emailSettings = configuration.GetSection("EmailSettings");
var email = emailSettings["Email"];
var password = emailSettings["Password"];
var smtp = emailSettings["Smtp"];
var port = emailSettings["Port"];


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(configuration);

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

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // 100 Mo
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 Mo
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings =
        {
            [".glb"] = "model/gltf-binary"
        }
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
