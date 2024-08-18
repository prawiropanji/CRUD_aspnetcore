using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using RepositoryContracts;
using Repositories;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();

//configure ILogger Provider
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog();
}

//add httplogging service to IoC container
builder.Services.AddHttpLogging(options => {
    options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
});



//add services to IoC container
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountryService>();

//add repositories to IoC container
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

//add dbcontext to IoC container
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.Logger.LogDebug("level Debug");
app.Logger.LogInformation("Level Information");
app.Logger.LogWarning("Level Warning");
app.Logger.LogError("Level Error");
app.Logger.LogCritical("Level Critical");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();
app.UseRotativa();



app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program { } //make the Program accessible to other class
