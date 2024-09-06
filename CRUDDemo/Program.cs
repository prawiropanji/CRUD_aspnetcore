using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using RepositoryContracts;
using Repositories;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using CRUDDemo.Filters.ActionFilters;
using CRUDDemo;
using CRUDDemo.Middleware;

var builder = WebApplication.CreateBuilder(args);




//configure serilog
builder.Host.UseSerilog((HostBuilderContext ctx, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(ctx.Configuration) //its mean serilog configuration is loaded form appsetting.json/appsettings.environment.json
        .ReadFrom.Services(service); //read services in IoC container and make available to serilog, its mean any serilog sinks can access service if required
});


////configure ILogger Provider 
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//if (OperatingSystem.IsWindows())
//{
//    builder.Logging.AddEventLog();
//}

////add httplogging service to IoC container
//builder.Services.AddHttpLogging(options =>
//{
//    options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
//});


builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseSerilogRequestLogging();

//app.UseHttpLogging();


app.UseRotativa();



app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program { } //make the Program accessible to other class
