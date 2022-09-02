
using BetBookData.Models;
using BetBookData.Interfaces;
using Microsoft.Extensions.Logging;
using BetBookDbAccess;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

namespace BetBookData.Data;

#nullable enable

public class ParleyBetData : IParleyBetData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<ParleyBetData> _logger;
    private readonly IConfiguration _configuration;

    public ParleyBetData(ISqlConnection db, ILogger<ParleyBetData> logger, IConfiguration configuration)
    {
        _db = db;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<IEnumerable<ParleyBetModel>> GetBettorParleyBetsUnpaid(int _bettorId)
    {
        _logger.LogInformation("Get Bettor Parley Bets Unpaid Call / ParleyBetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string sqlQuery = 
            $@"select * from dbo.ParleyBets where BettorId = {_bettorId} and ParleyPayoutStatus = 'UNPAID';";

        IEnumerable<ParleyBetModel> bettorParleyBetsUnpaid = 
            await connection.QueryAsync<ParleyBetModel>(sqlQuery);

        try
        {
            IEnumerable<TeamModel> teams = 
            await connection.QueryAsync<TeamModel>(@"select * from dbo.Teams;");
            IEnumerable<GameModel> games = 
                await connection.QueryAsync<GameModel>($@"select * from dbo.Games;");
            IEnumerable<BetModel> bets = 
                await connection.QueryAsync<BetModel>(
                    $@"select * from dbo.Bets where BettorId = {_bettorId} and PayoutStatus = 'PARLEY';");

            foreach (ParleyBetModel parleyBet in bettorParleyBetsUnpaid)
            {
                parleyBet.Bets.Add(bets.Where(b => b.Id == parleyBet.Bet1Id).FirstOrDefault()!);
                parleyBet.Bets.Add(bets.Where(b => b.Id == parleyBet.Bet2Id).FirstOrDefault()!);

                if (parleyBet.Bet3Id != 0)
                    parleyBet.Bets.Add(bets.Where(b => b.Id == parleyBet.Bet3Id).FirstOrDefault()!);
                if (parleyBet.Bet4Id != 0)
                    parleyBet.Bets.Add(bets.Where(b => b.Id == parleyBet.Bet4Id).FirstOrDefault()!);
                if (parleyBet.Bet5Id != 0)
                    parleyBet.Bets.Add(bets.Where(b => b.Id == parleyBet.Bet5Id).FirstOrDefault()!);

                foreach (BetModel bet in parleyBet.Bets)
                {
                    bet.Game = games.Where(g => g.Id == bet.GameId).FirstOrDefault();
                    bet.Game!.AwayTeam = teams.Where(t => t.Id == bet.Game.AwayTeamId).FirstOrDefault();
                    bet.Game!.HomeTeam = teams.Where(t => t.Id == bet.Game.HomeTeamId).FirstOrDefault();
                    bet.ChosenWinner = teams.Where(t => t.Id == bet.ChosenWinnerId).FirstOrDefault();
                }
            }
        }
        catch(Exception ex)
        {
            _logger.LogInformation(
                ex, "Failed To Populate Bettor Parley Bets Unpaid / ParleyBetData");
        }

        return bettorParleyBetsUnpaid;
    }

    public async Task<IEnumerable<ParleyBetModel>> GetInProgressParleyBets()
    {
        _logger.LogInformation("Get In Progress Parley Bets Call / ParleyBetData");

        IEnumerable<ParleyBetModel> inProgressParleyBets;
        IEnumerable<GameModel>? games;
        IEnumerable<TeamModel>? teams;

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string getInProgressParleyBetsSqlQuery = 
            @"select * from dbo.ParleyBets where ParleyBetStatus = 'IN_PROGRESS';";

        inProgressParleyBets = 
                await connection.QueryAsync<ParleyBetModel>(getInProgressParleyBetsSqlQuery);
        try
        {
            games = await connection.QueryAsync<GameModel>(@"select * from dbo.Games;");
            teams = await connection.QueryAsync<TeamModel>(@"select * from dbo.Teams;");

            foreach (ParleyBetModel parleyBet in inProgressParleyBets)
            {
                int highestBetId = parleyBet.Bet3Id == 0 ? parleyBet.Bet2Id
                                : parleyBet.Bet4Id == 0 ? parleyBet.Bet3Id
                                : parleyBet.Bet5Id == 0 ? parleyBet.Bet4Id
                                : parleyBet.Bet5Id;

                string getBetsSqlQuery = $@"select * 
                                            from dbo.Bets where Id >= {parleyBet.Bet1Id} 
                                            and Id <= {highestBetId};";

                parleyBet.Bets = (List<BetModel>)await connection.QueryAsync<BetModel>(getBetsSqlQuery);

                foreach (BetModel bet in parleyBet.Bets)
                {
                    bet.Game = games.Where(g => g.Id == bet.GameId).FirstOrDefault();
                    bet.Game!.AwayTeam = teams.Where(t => t.Id == bet.Game.AwayTeamId).FirstOrDefault();
                    bet.Game!.HomeTeam = teams.Where(t => t.Id == bet.Game.HomeTeamId).FirstOrDefault();
                    bet.ChosenWinner = teams.Where(t => t.Id == bet.ChosenWinnerId).FirstOrDefault();
                }
            }
        }
        catch(Exception ex)
        {
          _logger.LogInformation(
                ex, "Failed To Populate In Progress Parley Bets / ParleyBetData");
        }

        return inProgressParleyBets;
    }

    public async Task<int> InsertParleyBet(ParleyBetModel _parleyBet)
    {
        _logger.LogInformation("Insert Parley Bet Call / ParleyBetData");

        _parleyBet.Bet1Id = _parleyBet.Bets[0].Id;
        _parleyBet.Bet2Id = _parleyBet.Bets[1].Id;
        _parleyBet.Bet3Id = _parleyBet.Bets.Count > 2 ? _parleyBet.Bets[2].Id : 0;
        _parleyBet.Bet4Id = _parleyBet.Bets.Count > 3 ? _parleyBet.Bets[3].Id : 0;
        _parleyBet.Bet5Id = _parleyBet.Bets.Count > 4 ? _parleyBet.Bets[4].Id : 0;

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        var parameters = new DynamicParameters();

        parameters.Add("@Bet1Id", _parleyBet.Bet1Id);
        parameters.Add("@Bet2Id", _parleyBet.Bet2Id);
        parameters.Add("@Bet3Id", _parleyBet.Bet3Id);
        parameters.Add("@Bet4Id", _parleyBet.Bet4Id);
        parameters.Add("@Bet5Id", _parleyBet.Bet5Id);
        parameters.Add("@BettorId", _parleyBet.BettorId);
        parameters.Add("@BetAmount", _parleyBet.BetAmount);
        parameters.Add("@BetPayout", _parleyBet.BetPayout);
        parameters.Add("@ParleyBetStatus", _parleyBet.ParleyBetStatus.ToStringFast());
        parameters.Add("@ParleyPayoutStatus", _parleyBet.ParleyPayoutStatus.ToStringFast());

        string insertParleyBetSqlQuery =
                $@"insert into dbo.ParleyBets 
                    (Bet1Id, Bet2Id, Bet3Id, Bet4Id, Bet5Id, BettorId, BetAmount, BetPayout, ParleyBetStatus, ParleyPayoutStatus)
                    output Inserted.Id
                    values (@Bet1Id, @Bet2Id, @Bet3Id, @Bet4Id, @Bet5Id, @BettorId, @BetAmount, @BetPayout, @ParleyBetStatus, @ParleyPayoutStatus);";

        connection.Open();
        using var trans = connection.BeginTransaction();

        _parleyBet.Id =
            await connection.QuerySingleAsync<int>(
                insertParleyBetSqlQuery, parameters, transaction: trans);

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_parleyBet.BettorId};", transaction: trans);
            user.AccountBalance -= _parleyBet.BetAmount;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_parleyBet.BettorId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance += _parleyBet.BetAmount;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Parley Bet...Transaction Rolled Back / ParleyBetData");
            trans.Rollback();
        }

        return _parleyBet.Id;
    }

    public async Task UpdateParleyBet(ParleyBetModel parleyBet)
    {
        _logger.LogInformation(message: "Update Parley Bet Call / ParleyBetData");

        string parleyBetStatus = parleyBet.ParleyBetStatus.ToStringFast();
        string parleyPayoutStatus = parleyBet.ParleyPayoutStatus.ToStringFast();

        try
        {
            await _db.SaveData("dbo.spParleyBets_Update", new
            {
                parleyBet.Id,
                parleyBet.Bet1Id,
                parleyBet.Bet2Id,
                parleyBet.Bet3Id,
                parleyBet.Bet4Id,
                parleyBet.Bet5Id,
                parleyBet.BettorId,
                parleyBet.BetAmount,
                parleyBet.BetPayout,
                parleyBetStatus,
                parleyPayoutStatus
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Parley Bet / ParleyBetData");
        }
    }

    public async Task<bool> PayoutUnpaidWinningParleyBets(decimal _totalPendingParleyPayout, int _userId)
    {
        _logger.LogInformation(message: "Payout Unpaid Winning Parley Bets Call / ParleyBetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        using var trans = connection.BeginTransaction();

        string sqlQuery =
            $@"update dbo.ParleyBets 
                set ParleyPayoutStatus = 'PAID' where BettorId = {_userId} and ParleyBetStatus = 'WINNER' and ParleyPayoutStatus = 'UNPAID';";

        await connection.ExecuteAsync(sqlQuery, transaction: trans);

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_userId};", transaction: trans);
            user.AccountBalance += _totalPendingParleyPayout;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_userId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance -= _totalPendingParleyPayout;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(
                ex, $"Failed To Payout Unpaid Winning Parley Bets For Bettor Id# {_userId}...Transaction Rolled Back / ParleyBetData");
            trans.Rollback();
            return false;
        }
    }

    public async Task<bool> PayoutUnpaidPushParleyBets(decimal _totalPendingParleyRefund, int _userId)
    {
        _logger.LogInformation(message: "Payout Unpaid Push Parley Bets Call / ParleyBetData");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        connection.Open();
        using var trans = connection.BeginTransaction();

        string sqlQuery =
            $@"update dbo.ParleyBets 
                set ParleyPayoutStatus = 'PAID' where BettorId = {_userId} and ParleyBetStatus = 'PUSH' and ParleyPayoutStatus = 'UNPAID';";

        await connection.ExecuteAsync(sqlQuery, transaction: trans);

        try
        {
            UserModel user =
                await connection.QueryFirstOrDefaultAsync<UserModel>(
                    $"select * from dbo.Users where Id = {_userId};", transaction: trans);
            user.AccountBalance += _totalPendingParleyRefund;
            await connection.ExecuteAsync(
                $"update dbo.Users set AccountBalance = {user.AccountBalance} where Id = {_userId};", transaction: trans);

            HouseAccountModel houseAccount =
                await connection.QueryFirstOrDefaultAsync<HouseAccountModel>(
                    $"select * from dbo.HouseAccount;", transaction: trans);
            houseAccount.AccountBalance -= _totalPendingParleyRefund;
            await connection.ExecuteAsync(
                $"update dbo.HouseAccount set AccountBalance = {houseAccount.AccountBalance};", transaction: trans);

            trans.Commit();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(
                ex, $"Failed To Payout Unpaid Push Bets For Bettor Id# {_userId}...Transaction Rolled Back / ParleyBetData");
            trans.Rollback();
            return false;
        }
    }
}

#nullable restore
