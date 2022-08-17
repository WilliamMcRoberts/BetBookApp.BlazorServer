using BetBookData.Interfaces;
using BetBookData.Models;

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
            return Results.Problem(ex.Message);
        }
    }
}
