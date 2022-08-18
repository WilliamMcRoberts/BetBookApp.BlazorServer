using System.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using Microsoft.Extensions.Configuration;

namespace BetBookData.Helpers;

#nullable enable

public static class TransactionHelpers
{

    public static async Task<bool> CreateBetTransaction(this UserModel user,
                                                        BetModel bet,
                                                        HouseAccountModel houseAccount, 
                                                        IConfiguration config, 
                                                        IUserData userData,
                                                        IHouseAccountData houseData, 
                                                        IBetData betData)
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
            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine(ex.Message);
            return false;
        }
    }


    public static async Task<bool> PayoutBetsTransaction(this UserModel user,
                                                        List<BetModel> bettorBetsUnpaid,
                                                        HouseAccountModel houseAccount,
                                                        decimal totalPendingPayout, 
                                                        IConfiguration config,
                                                        IUserData userData,
                                                        IHouseAccountData houseData, 
                                                        IBetData betData)
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

            user.AccountBalance += totalPendingPayout;
            houseAccount.AccountBalance -= totalPendingPayout;

            await houseData.UpdateHouseAccount(houseAccount);
            await userData.UpdateUserAccountBalance(user);

            trans.Commit();
            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine($"Error: {ex.Message}");
            return false;   
        }
    }

    public static async Task<bool> CreateParleyBetTransaction(this UserModel user,
                                                              ParleyBetModel parleyBet,
                                                              HouseAccountModel houseAccount,
                                                              IConfiguration config, 
                                                              IUserData userData, 
                                                              IHouseAccountData houseData,
                                                              IParleyBetData parleyData)
    {
        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(config.GetConnectionString("BetBookDB"));

        connection.Open();

        using var trans = connection.BeginTransaction();

        try
        {
            await userData.UpdateUserAccountBalance(user);
            await houseData.UpdateHouseAccount(houseAccount);
            await parleyData.InsertParleyBet(parleyBet);

            trans.Commit();
            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static async Task<bool> PayoutParleyBetsTransaction(this UserModel user, 
                                                               List<ParleyBetModel> bettorParleyBetsUnpaid,
                                                               HouseAccountModel houseAccount,
                                                               decimal totalPendingParleyPayout, 
                                                               IConfiguration config,
                                                               IUserData userData, 
                                                               IHouseAccountData houseData,
                                                               IParleyBetData parleyData)
    {
        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(config.GetConnectionString("BetBookDB"));

        connection.Open();

        using var trans = connection.BeginTransaction();

        try
        {
            foreach (ParleyBetModel pb in bettorParleyBetsUnpaid)
            {
                pb.ParleyPayoutStatus = ParleyPayoutStatus.PAID;

                await parleyData.UpdateParleyBet(pb);
            }

            user.AccountBalance += totalPendingParleyPayout;
            houseAccount.AccountBalance -= totalPendingParleyPayout;

            await houseData.UpdateHouseAccount(houseAccount);
            await userData.UpdateUserAccountBalance(user);

            trans.Commit();
            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();

            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}


#nullable restore
