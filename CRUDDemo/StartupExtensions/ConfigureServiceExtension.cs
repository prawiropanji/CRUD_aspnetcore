using CRUDDemo.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDDemo
{
    public static class ConfigureServiceExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HeaderActionFilter>();

            services.AddControllersWithViews(options =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<HeaderActionFilter>>();
                options.Filters.Add(new HeaderActionFilter(logger) { Key = "x-key-from-global", Value = "x-value-from-global", Order = 2 });
            });


            //add services to IoC container
            services.AddScoped<IPersonsService, PersonsService>();
            services.AddScoped<ICountriesService, CountryService>();

            //add repositories to IoC container 
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            //add dbcontext to IoC container
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        }
    }
}
