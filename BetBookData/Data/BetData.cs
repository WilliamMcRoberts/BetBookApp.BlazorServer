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
    private readonly IConfiguration _config;
    private readonly ILogger<BetData> _logger;

    public BetData(ISqlConnection db, IConfiguration config, ILogger<BetData> logger)
    {
        _db = db;
        _config = config;
        _logger = logger;
    }

    public async Task<IEnumerable<BetModel>> GetBets() 
    {
        _logger.LogInformation( "Get Bets Call");

        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAll", new { });
    }

    public async Task<BetModel?> GetBet(int betId)
    {
        _logger.LogInformation( "Get Bet Call");

        var results = await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_Get", new
            {
                Id = betId
            });

        return results.FirstOrDefault();
    }

    public async Task<int> InsertBet(BetModel bet)
    {
        string betStatus = BetStatus.IN_PROGRESS.ToStringFast();
        string payoutStatus;

        payoutStatus = bet.PayoutStatus == PayoutStatus.PARLEY ? PayoutStatus.PARLEY.ToStringFast() 
                       : PayoutStatus.UNPAID.ToStringFast();

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _config.GetConnectionString("BetBookDB"));

        var p = new DynamicParameters();

        p.Add("@BetAmount", bet.BetAmount);
        p.Add("@BetPayout", bet.BetPayout);
        p.Add("@BettorId", bet.BettorId);
        p.Add("@GameId", bet.GameId);
        p.Add("@ChosenWinnerId", bet.ChosenWinnerId);
        p.Add("@BetStatus", betStatus);
        p.Add("@PayoutStatus", payoutStatus);
        p.Add("@PointSpread", bet.PointSpread);
        p.Add( "@Id", 0, dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        _logger.LogInformation( "Insert Bet Call");
        try
        {
            await connection.ExecuteAsync(
                "dbo.spBets_Insert", p, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Bet");
        }

        return bet.Id = p.Get<int>("@Id");
    }

    public async Task UpdateBet(BetModel bet)
    {
        string betStatus = bet.BetStatus.ToStringFast();
        string payoutStatus = bet.PayoutStatus.ToStringFast();

        _logger.LogInformation( "Update Bet Call");
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
            _logger.LogInformation(ex, "Failed To Update Bet");
        }
    }

    public async Task DeleteBet(int id)
    {
        _logger.LogInformation( "Delete Bet Call");
        try
        {
            await _db.SaveData( "dbo.spBets_Delete", new
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Delete Bet");
        }
    }
}

#nullable restore
