using BetBookData.Interfaces;
using BetBookData.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using BetBookData.DbAccess;

namespace BetBookData.Data;

#nullable enable

public class BetData : IBetData
{

    private readonly ISqlConnection _db;
    private readonly IConfiguration _config;

    /// <summary>
    /// BetData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public BetData(ISqlConnection db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    /// <summary>
    /// Async method calls the spBets_GetAll stored procedure to retrieve 
    /// all bets in the database
    /// </summary>
    /// <returns>IEnumerable of BetModel represents all bets in the database</returns>
    public async Task<IEnumerable<BetModel>> GetBets() => 
        await _db.LoadData<BetModel, dynamic>( "dbo.spBets_GetAll", new { });

    /// <summary>
    /// Async method calls spBets_Get stored procedure which retrieves one 
    /// bet by bet id
    /// </summary>
    /// <param name="betId">int represents the id of the bet being retrieved from the database</param>
    /// <returns>BetModel represents the bet being retrieved from the database</returns>
    public async Task<BetModel?> GetBet(int betId)
    {
        var result = await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_Get", new
            {
                Id = betId
            });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spBets_Insert stored procedure to insert one bet 
    /// entry into the database
    /// </summary>
    /// <param name="bet">BetModel represents a bet to insert into the database</param>
    /// <returns></returns>
    public async Task<int> InsertBet(BetModel bet)
    {
        string betStatus = BetStatus.IN_PROGRESS.ToString();
        string payoutStatus;

        payoutStatus = bet.PayoutStatus == PayoutStatus.PARLEY ? PayoutStatus.PARLEY.ToString() 
                       : PayoutStatus.UNPAID.ToString();

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
        p.Add( "@Id", 0, dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "dbo.spBets_Insert", p, commandType: CommandType.StoredProcedure);

        bet.Id = p.Get<int>("@Id");

        return bet.Id;
    }

    /// <summary>
    /// Async method calls the spBets_Update stored procedure if the bet is not a push
    /// and calls the spBets_UpdatePush stored procedure if the bet is a push
    /// </summary>
    /// <param name="bet">BetModel represents a bet being updated into the database</param>
    /// <returns></returns>
    public async Task UpdateBet(BetModel bet)
    {
        string betStatus = bet.BetStatus.ToString();
        string payoutStatus = bet.PayoutStatus.ToString();

        if (bet.FinalWinnerId != 0)
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
                payoutStatus
            });

            return;
        }

        await _db.SaveData("dbo.spBets_UpdatePush", new
        {
            bet.Id,
            bet.BetAmount,
            bet.BetPayout,
            bet.BettorId,
            bet.GameId,
            bet.ChosenWinnerId,
            betStatus,
            payoutStatus
        });

    }

    /// <summary>
    /// Async method calls the spBets_Delete stored procedure which deletes one bet
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the id of the bet to be deleted from the database</param>
    /// <returns></returns>
    public async Task DeleteBet(int id)
    {
        await _db.SaveData(
        "dbo.spBets_Delete", new
        {
            Id = id
        });
    }
}

#nullable restore
