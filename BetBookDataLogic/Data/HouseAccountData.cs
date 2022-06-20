using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public class HouseAccountData : IHouseAccountData
{
    private readonly ISqlConnection _db;

    /// <summary>
    /// HouseAccountData Constructor
    /// </summary>
    /// <param name="db"></param>
    public HouseAccountData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Method calls spHouseAccount_Get stored procedure which retrieves the 
    /// house account
    /// </summary>
    /// <param name="id"></param>
    /// <returns>HouseAccountModel</returns>
    public async Task<HouseAccountModel?> GetHouseAccount()
    {
        var result = await _db.LoadData<HouseAccountModel, dynamic>(
            "dbo.spHouseAccount_Get", new { });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Method calls the spHouseAccount_Update stored procedure to update
    /// the house account
    /// </summary>
    /// <param name="houseAccount"></param>
    /// <returns></returns>
    public async Task UpdateHouseAccount(HouseAccountModel houseAccount)
    {
        await _db.SaveData("dbo.spHouseAccount_Update", new
        {
            houseAccount.AccountBalance
        });
    }
}
