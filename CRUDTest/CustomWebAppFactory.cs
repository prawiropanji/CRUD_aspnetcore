using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest
{
    //Createing test server based on main Program.cs (same configuration, middleware, and service real application uses)
    //
    public class CustomWebAppFactory:WebApplicationFactory<Program>
    {
        //mengubah konfigurasi test server yang mimicking real application untuk kebutuhan testing
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);


            builder.UseEnvironment("Test");


            //Remove DbContext service
            builder.ConfigureServices(services =>
            {
                ServiceDescriptor? descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if(descriptor != null)
                {
                    //remove from IoC container
                    services.Remove(descriptor);
                }

                //add new DbContext use in-memmory
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryTestDatabase") );

            });


            

        }
    }
}
