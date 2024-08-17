using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using RepositoryContracts;
using Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CRUDTest")]


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


//add services to IoC container
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountryService>();

//add repositories to IoC container
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

//add dbcontext to IoC container
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRotativa();


app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically
