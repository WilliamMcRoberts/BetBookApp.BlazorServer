using System.Data;
using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BetBookData.Services;

#nullable enable

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public TransactionService(ILogger<TransactionService> logger,
                              IConfiguration configuration,
                              IMediator mediator)
    {
        _logger = logger;
        _configuration = configuration;
        _mediator = mediator;
    }

    public async Task<bool> CreateBetTransaction(UserModel user, BetModel bet)
    {
        HouseAccountModel? houseAccount = 
            await _mediator.Send(new GetHouseAccountQuery());

        user.AccountBalance -= bet.BetAmount;
        houseAccount!.AccountBalance += bet.BetAmount;

        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection...");

        using var transaction = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction (CreateBetTransaction)...");

        try
        {
            await _mediator.Send(new UpdateUserAccountBalanceCommand(user));
            await _mediator.Send(new UpdateHouseAccountCommand(houseAccount));
            await _mediator.Send(new InsertBetCommand(bet));

            transaction.Commit();
            _logger.LogInformation("Transaction Committed (CreateBetTransaction)...");
            return true;
        }

        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back (CreateBetTransaction)...");

            return false;
        }
    }


    public async Task<bool> PayoutBetsTransaction(
        UserModel user, List<BetModel> bettorBetsUnpaid, decimal totalPendingPayout)
    {
        HouseAccountModel? houseAccount =
            await _mediator.Send(new GetHouseAccountQuery());

        user.AccountBalance += totalPendingPayout;
        houseAccount!.AccountBalance -= totalPendingPayout;

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection...");

        using var transaction = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction (PayoutBetsTransaction)...");

        try
        {
            foreach (var bet in bettorBetsUnpaid)
            {
                bet.PayoutStatus = PayoutStatus.PAID;

                await _mediator.Send(new UpdateBetCommand(bet));
            }

            await _mediator.Send(new UpdateHouseAccountCommand(houseAccount));
            await _mediator.Send(new UpdateUserAccountBalanceCommand(user));

            transaction.Commit();
            _logger.LogInformation("Transaction Committed (PayoutBetsTransaction)...");

            return true;
        }

        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back (PayoutBetsTransaction)...");

            return false;
        }
    }

    public async Task<bool> CreateParleyBetTransaction(
            UserModel user, ParleyBetModel parleyBet)
    {
        HouseAccountModel? houseAccount =
            await _mediator.Send(new GetHouseAccountQuery());

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection...");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction (CreateParleyBetTransaction)...");

        try
        {
            user.AccountBalance -= parleyBet.BetAmount;
            houseAccount!.AccountBalance += parleyBet.BetAmount;

            await _mediator.Send(new UpdateUserAccountBalanceCommand(user));
            await _mediator.Send(new UpdateHouseAccountCommand(houseAccount));
            await _mediator.Send(new InsertParleyBetCommand(parleyBet));

            trans.Commit();
            _logger.LogInformation("Transaction Committed (CreateParleyBetTransaction)");

            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back (CreateParleyBetTransaction)");

            return false;
        }
    }

    public async Task<bool> PayoutParleyBetsTransaction(
            UserModel user, List<ParleyBetModel> bettorParleyBetsUnpaid,
            decimal totalPendingParleyPayout)
    {
        HouseAccountModel? houseAccount =
            await _mediator.Send(new GetHouseAccountQuery());

        using IDbConnection connection = new System.Data.SqlClient
                    .SqlConnection(_configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        _logger.LogInformation("Opened Connection...");

        using var trans = connection.BeginTransaction();
        _logger.LogInformation("Begin Transaction (PayoutParleyBetsTransaction)...");

        try
        {
            foreach (var parleyBet in bettorParleyBetsUnpaid)
            {
                parleyBet.ParleyPayoutStatus = ParleyPayoutStatus.PAID;

                await _mediator.Send(new UpdateParleyBetCommand(parleyBet));
            }

            user.AccountBalance += totalPendingParleyPayout;
            houseAccount!.AccountBalance -= totalPendingParleyPayout;

            await _mediator.Send(new UpdateHouseAccountCommand(houseAccount));
            await _mediator.Send(new UpdateUserAccountBalanceCommand(user));

            trans.Commit();
            _logger.LogInformation("Transaction Committed (PayoutParleyBetsTransaction)");

            return true;
        }

        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogInformation(ex, "Transaction Rolled Back (PayoutParleyBetsTransaction)");

            return false;
        }
    }
}


#nullable restore
