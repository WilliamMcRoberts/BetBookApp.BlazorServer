using BetBookData.Interfaces;
using BetBookDbAccess;
using BetBookData.Models;

namespace BetBookData.Data;

public class HouseAccountData : IHouseAccountData
{
    private readonly ISqlConnection _db;

    /// <summary>
    /// HouseAccountData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public HouseAccountData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method retrieves the house account
    /// </summary>
    /// <returns>
    /// HouseAccountModel represents the house account
    /// </returns>
    public async Task<HouseAccountModel?> GetHouseAccount()
    {
        var result = await _db.LoadData<HouseAccountModel, dynamic>(
            "dbo.spHouseAccount_Get", new { });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spHouseAccount_Update stored procedure to update
    /// the house account
    /// </summary>
    /// <param name="houseAccount">HouseAccountModel represents the house account</param>
    /// <returns></returns>
    public async Task UpdateHouseAccount(HouseAccountModel houseAccount)
    {
        await _db.SaveData("dbo.spHouseAccount_Update", new
        {
            houseAccount.AccountBalance
        });
    }
}
