using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookDbAccess;
using Microsoft.Extensions.Logging;

namespace BetBookData.Data;

#nullable enable

public class HouseAccountData : IHouseAccountData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<HouseAccountData> _logger;

    public HouseAccountData(ISqlConnection db, ILogger<HouseAccountData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<HouseAccountModel?> GetHouseAccount()
    {
        _logger.LogInformation("Get House Account Call");


        var result = await _db.LoadData<HouseAccountModel, dynamic>(
            "dbo.spHouseAccount_Get", new { });

        return result.FirstOrDefault();
    }

    public async Task UpdateHouseAccount(HouseAccountModel houseAccount)
    {
        _logger.LogInformation( "Update House Account Call");

        try
        {
            await _db.SaveData("dbo.spHouseAccount_Update", new
            {
                houseAccount.AccountBalance
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update House Account");
        }
    }
}

#nullable restore
