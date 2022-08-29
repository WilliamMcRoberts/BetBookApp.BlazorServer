using BetBookData.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookMinApi;
using BetBookMinApi.Api;
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

    builder.Host.UseSerilog();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();


    app.ConfigureGamesApi();
    app.ConfigureTeamsApi();
    app.ConfigureUsersApi();
    app.ConfigureBetsApi();
    app.ConfigureParleyBetsApi();
    app.ConfigureHouseAccountApi();

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


