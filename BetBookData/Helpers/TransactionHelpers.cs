using System.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using Microsoft.Extensions.Configuration;

namespace BetBookData.Helpers;

public static class TransactionHelpers
{
    /// <summary>
    /// Async static transaction method to transfer funds from user account to 
    /// house account, then insert bet into the database...if both transactions do not 
    /// complete...transaction will get rolled back
    /// </summary>
    /// <param name="user">UserModel represents the current user</param>
    /// <param name="houseAccount">HouseAccountModel represents the house account</param>
    /// <param name="bet">BetModel represents the bet to insert into the database</param>
    /// <param name="config">IConfiguration</param>
    /// <param name="userData">IUserData</param>
    /// <param name="houseData">IHouseAccountData</param>
    /// <param name="betData">IBetData</param>
    /// <returns></returns>
    public static async Task CreateBetTransaction(
       this UserModel user, HouseAccountModel houseAccount, 
        BetModel bet, IConfiguration config, IUserData userData, 
        IHouseAccountData houseData, IBetData betData)
    {
        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(config.GetConnectionString("BetBookDB"));

        connection.Open();

        using var trans = connection.BeginTransaction();

        try
        {
            await userData.UpdateUserAccountBalance(user);
            await houseData.UpdateHouseAccount(houseAccount);
            await betData.InsertBet(bet);

            trans.Commit();

        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Async static transaction method to transfer funds from house account to 
    /// user account...if both transactions do not complete...transaction
    /// will get rolled back
    /// </summary>
    /// <param name="user">UserModel represents current user</param>
    /// <param name="houseAccount">HouseAccountModel represents the house account</param>
    /// <param name="bettorBetsUnpaid">
    /// List<BetModel> represents a list of bets that 
    /// have payout status "UNPAID"
    /// </param>
    /// <param name="config">IConfiguration</param>
    /// <param name="userData">IUserData</param>
    /// <param name="houseData">IHouseData</param>
    /// <param name="betData">IBetData</param>
    /// <returns></returns>
    public static async Task PayoutBetsTransaction(
        this UserModel user, HouseAccountModel houseAccount, 
        List<BetModel> bettorBetsUnpaid, IConfiguration config, 
        IUserData userData,IHouseAccountData houseData, IBetData betData)
    {
        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(config.GetConnectionString("BetBookDB"));

        connection.Open();

        using var trans = connection.BeginTransaction();

        try
        {
            foreach (BetModel bet in bettorBetsUnpaid)
            {
                bet.PayoutStatus = PayoutStatus.PAID;

                await betData.UpdateBet(bet);
            }

            await houseData.UpdateHouseAccount(houseAccount);
            await userData.UpdateUserAccountBalance(user);

            trans.Commit();
        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
