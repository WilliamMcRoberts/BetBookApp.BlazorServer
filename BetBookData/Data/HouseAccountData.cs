using BetBookData.DbAccess;
using BetBookData.Interfaces;
using BetBookData.Models;
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
        _logger.LogInformation(message: "Http Get / Get House Account");


        var result = await _db.LoadData<HouseAccountModel, dynamic>(
            "dbo.spHouseAccount_Get", new { });

        return result.FirstOrDefault();
    }

    public async Task UpdateHouseAccount(HouseAccountModel houseAccount)
    {
        _logger.LogInformation(message: "Http Put / Update House Account");


        await _db.SaveData("dbo.spHouseAccount_Update", new
        {
            houseAccount.AccountBalance
        });
    }
}

#nullable restore
