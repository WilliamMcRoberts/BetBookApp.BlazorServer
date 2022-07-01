using System.Data;
using BetBookData.DataLogic.Interfaces;
using BetBookData.DbAccess;
using BetBookData.Models;
using Microsoft.Extensions.Configuration;

namespace BetBookData.DataLogic;
public class Tranactions : ITranactions
{
    private readonly ISqlConnection _db;
    private readonly IConfiguration _config;
    private readonly IUserData _userData;
    private readonly IHouseAccountData _houseData;
    private readonly IBetData _betData;

    /// <summary>
    /// Transactions Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    /// <param name="config">IConfiguration represents Configuration class interface</param>
    /// <param name="userData">IUserData represents UserData class interface</param>
    /// <param name="houseData">IHouseData represents HouseData class interface</param>
    public Tranactions(ISqlConnection db,
                        IConfiguration config,
                        IUserData userData,
                        IHouseAccountData houseData,
                        IBetData betData)
    {
        _db = db;
        _config = config;
        _userData = userData;
        _houseData = houseData;
        _betData = betData;
    }

    /// <summary>
    /// Async transaction method to transfer funds from user account to 
    /// house account, then insert bet into the database...if both transactions do not 
    /// complete...transaction will get rolled back
    /// </summary>
    /// <param name="user">UserModel represents user of account being updated</param>
    /// <param name="houseAccount">HouseAccountModel represents the house account</param>
    /// <returns></returns>
    public async Task CreateBetTransaction(UserModel user, HouseAccountModel houseAccount, BetModel bet)
    {
        using (IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_config.GetConnectionString("BetBookDB")))
        {
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                try
                {
                    await _userData.UpdateUserAccountBalance(user);
                    await _houseData.UpdateHouseAccount(houseAccount);
                    await _betData.InsertBet(bet);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    trans.Rollback();
                }
            }
        }
    }

    /// <summary>
    /// Async transaction method to transfer funds from house account to 
    /// user account...if both transactions do not complete...transaction
    /// will get rolled back
    /// </summary>
    /// <param name="user">UserModel represents user of account being updated</param>
    /// <param name="houseAccount">HouseAccountModel represents the house account</param>
    /// <returns></returns>
    public async Task PayoutBetsTransaction(UserModel user, HouseAccountModel houseAccount, List<BetModel> bettorBetsUnpaid)
    {
        using (IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_config.GetConnectionString("BetBookDB")))
        {
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (BetModel bet in bettorBetsUnpaid)
                    {
                        bet.PayoutStatus = PayoutStatus.PAID;

                        await _betData.UpdateBet(bet);
                    }

                    await _houseData.UpdateHouseAccount(houseAccount);
                    await _userData.UpdateUserAccountBalance(user);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    trans.Rollback();
                }
            }
        }
    }
}
