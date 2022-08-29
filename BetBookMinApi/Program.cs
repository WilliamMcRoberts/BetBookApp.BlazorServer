using BetBookData.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookMinApi.Api;
using BetBookMinApi.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
try
{
    Log.Information("Application Starting...");
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    var app = builder.Build();

    app.ConfigureSwagger();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();

    app.ConfigureApi();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.OrdinalIgnoreCase)) throw;
    Log.Fatal(ex, "The Host Stopped Unexpectedly...");
}
finally
{
    Log.CloseAndFlush();
}


