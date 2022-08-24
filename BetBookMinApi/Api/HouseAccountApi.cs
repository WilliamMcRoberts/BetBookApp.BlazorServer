using BetBookData.Interfaces;
using BetBookData.Models;
using Serilog;

namespace BetBookMinApi.Api;

public static class HouseAccountApi
{
    public static void ConfigureHouseAccountApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/HouseAccount", GetHouseAccount).WithName("GetHouseAccount");
        app.MapPut("/HouseAccount", UpdateHouseAccount).WithName("UpdateHouseAccount");
    }

    public static async Task<IResult> GetHouseAccount(IHouseAccountData data)
    {
        try
        {
            return Results.Ok(await data.GetHouseAccount());
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Get House Account");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateHouseAccount(HouseAccountModel account, IHouseAccountData data)
    {
        try
        {
            await data.UpdateHouseAccount(account);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Update House Account");

            return Results.Problem(ex.Message);
        }
    }
}
