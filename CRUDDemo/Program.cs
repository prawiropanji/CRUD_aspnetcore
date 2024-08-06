using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services to IoC container
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountryService>();

//add dbcontext to IoC container
builder.Services.AddDbContext<PersonDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRotativa();


app.UseStaticFiles();
app.MapControllers();
app.Run();
