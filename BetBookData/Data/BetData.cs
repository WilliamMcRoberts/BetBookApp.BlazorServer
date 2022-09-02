using BetBookData.Interfaces;
using BetBookData.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;
using BetBookDbAccess;

namespace BetBookData.Data;

#nullable enable

public class BetData : IBetData
{
    private readonly ISqlConnection _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BetData> _logger;

    public BetData(ISqlConnection db, IConfiguration config, ILogger<BetData> logger)
    {
        _db = db;
        _configuration = config;
        _logger = logger;
    }

    public async Task<IEnumerable<BetModel>> GetBetsOnCurrentGame(int _gameId)
    {
        _logger.LogInformation("Get Bets On Current Game Call / BetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string sqlQuery = $@"select * from dbo.Bets where GameId = {_gameId};";

        IEnumerable<BetModel> betsOnCurrentGame = 
            await connection.QueryAsync<BetModel>(sqlQuery);

        try
        {
            IEnumerable<TeamModel> teams = await connection.QueryAsync<TeamModel>(@"select * from dbo.Teams;");
            GameModel currentGame = await connection.QueryFirstOrDefaultAsync<GameModel>($@"select * from dbo.Games where Id = {_gameId};");
            foreach (BetModel bet in betsOnCurrentGame)
            {
                bet.Game = currentGame;
                bet.Game.AwayTeam = teams.Where(t => t.Id == currentGame.AwayTeamId).FirstOrDefault();
                bet.Game.HomeTeam = teams.Where(t => t.Id == currentGame.HomeTeamId).FirstOrDefault();
                bet.ChosenWinner = teams.Where(t => t.Id == bet.ChosenWinnerId).FirstOrDefault();
            }
        }
        catch(Exception ex)
        {
            _logger.LogInformation(
                ex, "Failed To Populate Bets On Current Game / BetData");
        }
        
        return betsOnCurrentGame;
    }

    public async Task<IEnumerable<BetModel>> GetBettorBetsUnpaid(int _bettorId)
    {
        _logger.LogInformation("Get Bettor Bets Unpaid Call / BetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string sqlQuery = $@"select * from dbo.Bets where BettorId = {_bettorId} and PayoutStatus = 'UNPAID';";

        IEnumerable<BetModel> bettorBets = await connection.QueryAsync<BetModel>(sqlQuery);

        try
        {
            IEnumerable<TeamModel> teams = await connection.QueryAsync<TeamModel>(@"select * from dbo.Teams;");
            IEnumerable<GameModel> games = await connection.QueryAsync<GameModel>($@"select * from dbo.Games;");
            foreach (BetModel bet in bettorBets)
            {
                bet.Game = games.Where(g => g.Id == bet.GameId).FirstOrDefault();
                bet.Game!.AwayTeam = teams.Where(t => t.Id == bet.Game.AwayTeamId).FirstOrDefault();
                bet.Game.HomeTeam = teams.Where(t => t.Id == bet.Game.HomeTeamId).FirstOrDefault();
                bet.ChosenWinner = teams.Where(t => t.Id == bet.ChosenWinnerId).FirstOrDefault();
            }
        }
        catch(Exception ex)
        {
            _logger.LogInformation(
                ex, "Failed To Populate Bettor Bets Unpaid / BetData");
        }
        

        return bettorBets;
    }

    public async Task<int> InsertBet(BetModel _bet)
    {
        _logger.LogInformation("Insert Bet Transaction Call / BetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        var parameters = new DynamicParameters();

        parameters.Add("@BetAmount", _bet.BetAmount);
        parameters.Add("@BetPayout", _bet.BetPayout);
        parameters.Add("@BettorId", _bet.BettorId);
        parameters.Add("@GameId", _bet.GameId);
        parameters.Add("@ChosenWinnerId", _bet.ChosenWinnerId);
        parameters.Add("@BetStatus", _bet.BetStatus.ToStringFast());
        parameters.Add("@PayoutStatus", _bet.PayoutStatus.ToStringFast());
        parameters.Add("@PointSpread", _bet.PointSpread);
        parameters.Add("@Id", 0, dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        string insertBetSqlQuery =
                $@"insert into dbo.Bets 
                    (BetAmount, BetPayout, BettorId, GameId, ChosenWinnerId, BetStatus, PayoutStatus, PointSpread)
                    output Inserted.Id
                    values (@BetAmount, @BetPayout, @BettorId, @GameId, @ChosenWinnerId, @BetStatus, @PayoutStatus, @PointSpread);";

        connection.Open();
        using var trans = connection.BeginTransaction();

        _bet.Id = await connection.QuerySingleAsync<int>(insertBetSqlQuery, parameters, transaction: trans);

        if (_bet.BetAmount == 0)
        {
            trans.Commit();
            return _bet.Id;
        }

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_bet.BettorId};", transaction: trans);
            user.AccountBalance -= _bet.BetAmount;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_bet.BettorId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance += _bet.BetAmount;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Bet And Update Account Balances...Transaction Rolled Back / BetData");
            trans.Rollback();
        }

        return _bet.Id;
    }

    public async Task UpdateBet(BetModel bet)
    {
        string betStatus = bet.BetStatus.ToStringFast();
        string payoutStatus = bet.PayoutStatus.ToStringFast();

        _logger.LogInformation("Update Bet Call");
        try
        {
            await _db.SaveData("dbo.spBets_Update", new
            {
                bet.Id,
                bet.BetAmount,
                bet.BetPayout,
                bet.BettorId,
                bet.GameId,
                bet.ChosenWinnerId,
                bet.FinalWinnerId,
                betStatus,
                payoutStatus,
                bet.PointSpread
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Bet / BetData");
        }
    }

    public async Task<bool> PayoutUnpaidWinningBets(decimal _totalPendingPayout, int _userId)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        using var trans = connection.BeginTransaction();

        string sqlQuery =
            $@"update dbo.Bets 
                set PayoutStatus = 'PAID' where BettorId = {_userId} and BetStatus = 'WINNER' and PayoutStatus = 'UNPAID';";

        await connection.ExecuteAsync(sqlQuery, transaction: trans);

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_userId};", transaction: trans);
            user.AccountBalance += _totalPendingPayout;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_userId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance -= _totalPendingPayout;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Failed To Payout Unpaid Winning Bets For Bettor Id# {_userId}...Transaction Rolled Back / BetData");
            trans.Rollback();
            return false;
        }
    }

    public async Task<bool> PayoutUnpaidPushBets(decimal _totalPendingRefund, int _userId)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        using var trans = connection.BeginTransaction();

        string sqlQuery =
            $@"update dbo.Bets 
                set PayoutStatus = 'PAID' where BettorId = {_userId} and BetStatus = 'PUSH' and PayoutStatus = 'UNPAID';";

        await connection.ExecuteAsync(sqlQuery, transaction: trans);

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_userId};", transaction: trans);
            user.AccountBalance += _totalPendingRefund;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_userId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance -= _totalPendingRefund;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"Failed To Payout Unpaid Push Bets For Bettor Id# {_userId}...Transaction Rolled Back / BetData");
            trans.Rollback();
            return false;
        }
    }
}

#nullable restore
