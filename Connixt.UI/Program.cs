using Connixt.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();

// session for 20 minutes
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// cookie auth with 20 min sliding expiration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });

// ApiClient: server-to-server calls to Connixt.Api
builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    var apiBase = builder.Configuration["ConnixtApi:BaseUrl"] ?? "https://localhost:7215/";
    client.BaseAddress = new Uri(apiBase);
    client.Timeout = TimeSpan.FromSeconds(300);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
