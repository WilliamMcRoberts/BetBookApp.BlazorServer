using System.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BetBookData.Services;

#nullable enable

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly IUserData _userData;
    private readonly IGameData _gameData;
    private readonly ITeamData _teamData;
    private readonly IBetData _betData;
    private readonly IParleyBetData _parleyData;
    private readonly IHouseAccountData _houseData;
    private readonly IConfiguration _configuration;

    public TransactionService(ILogger<TransactionService> logger,
                              IGameData gameData,
                              ITeamData teamData,
                              IBetData betData,
                              IParleyBetData parleyData,
                              IHouseAccountData houseData,
                              IConfiguration configuration,
                              IUserData userData)
    {
        _logger = logger;
        _gameData = gameData;
        _teamData = teamData;
        _betData = betData;
        _parleyData = parleyData;
        _houseData = houseData;
        _configuration = configuration;
        _userData = userData;
    }

    public async Task<bool> CreateBetTransaction(UserModel user, BetModel bet)
    {
        HouseAccountModel? houseAccount = await _houseData.GetHouseAccount();



        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection To Database");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction");

        try
        {
            user.AccountBalance -= bet.BetAmount;
            houseAccount!.AccountBalance += bet.BetAmount;

            await _userData.UpdateUserAccountBalance(user);
            await _houseData.UpdateHouseAccount(houseAccount!);
            await _betData.InsertBet(bet);

            trans.Commit();
            _logger.LogInformation("Transaction Committed");
            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back");

            return false;
        }
    }


    public async Task<bool> PayoutBetsTransaction(
            UserModel user, List<BetModel> bettorBetsUnpaid,
            decimal totalPendingPayout)
    {
        HouseAccountModel? houseAccount = await _houseData.GetHouseAccount();

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection To Database");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction");

        try
        {
            foreach (var bet in bettorBetsUnpaid)
            {
                bet.PayoutStatus = PayoutStatus.PAID;

                await _betData.UpdateBet(bet);
            }

            user.AccountBalance += totalPendingPayout;
            houseAccount!.AccountBalance -= totalPendingPayout;

            await _houseData.UpdateHouseAccount(houseAccount);
            await _userData.UpdateUserAccountBalance(user);

            trans.Commit();
            _logger.LogInformation("Transaction Committed");

            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back");

            return false;
        }
    }

    public async Task<bool> CreateParleyBetTransaction(
            UserModel user, ParleyBetModel parleyBet)
    {
        HouseAccountModel? houseAccount = await _houseData.GetHouseAccount();

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection To Database");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction");

        try
        {
            user.AccountBalance -= parleyBet.BetAmount;
            houseAccount!.AccountBalance += parleyBet.BetAmount;

            await _userData.UpdateUserAccountBalance(user);
            await _houseData.UpdateHouseAccount(houseAccount!);
            await _parleyData.InsertParleyBet(parleyBet);

            trans.Commit();
            _logger.LogInformation("Transaction Committed");

            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back");

            return false;
        }
    }

    public async Task<bool> PayoutParleyBetsTransaction(
            UserModel user, List<ParleyBetModel> bettorParleyBetsUnpaid,
            decimal totalPendingParleyPayout)
    {
        HouseAccountModel? houseAccount = await _houseData.GetHouseAccount();

        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection To Database");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction");

        try
        {
            foreach (var pb in bettorParleyBetsUnpaid)
            {
                pb.ParleyPayoutStatus = ParleyPayoutStatus.PAID;

                await _parleyData.UpdateParleyBet(pb);
            }

            user.AccountBalance += totalPendingParleyPayout;
            houseAccount!.AccountBalance -= totalPendingParleyPayout;

            await _houseData.UpdateHouseAccount(houseAccount);
            await _userData.UpdateUserAccountBalance(user);

            trans.Commit();
            _logger.LogInformation("Transaction Committed");

            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back");

            return false;
        }
    }
}


#nullable restore
