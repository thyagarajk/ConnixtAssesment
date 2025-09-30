using Connixt.Api.Middleware;
using Connixt.Api.Services;
using Connixt.Shared;
using Connixt.Shared.SoapDtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

// configuration
builder.Services.AddControllers();

// CORS: allow UI origin (adjust the port if your UI runs elsewhere)
builder.Services.AddCors(options =>
{
    options.AddPolicy("UiPolicy", pb =>
    {
        pb.WithOrigins("https://localhost:5002", "http://localhost:5002") // update if needed
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

// typed HttpClient for SoapClient
builder.Services.AddHttpClient<ISoapClient, SoapClient>(client =>
{
    // BaseAddress will be overridden by config inside SoapClient if needed
    // But adding default base ensures HttpClient is ready
    client.Timeout = TimeSpan.FromSeconds(300);
});

// DI for operation-level service
builder.Services.AddScoped<ISoapService, SoapService>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCors("UiPolicy");
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";  // This is the default; leaving empty serves at root
});
app.MapControllers();

app.Run();
