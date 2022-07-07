using BetBookData.Interfaces;
using BetBookDataAccess.DbAccess;
using BetBookData.Models;

namespace BetBookData.Data;

public class BetData : IBetData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// BetData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public BetData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spBets_GetAll stored procedure to retrieve 
    /// all bets in the database
    /// </summary>
    /// <returns>IEnumerable of BetModel represents all bets in the database</returns>
    public async Task<IEnumerable<BetModel>> GetBets()
    {
        return await _db.LoadData<BetModel, dynamic>(
        "dbo.spBets_GetAll", new { });
    }

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
    /// Async method calls spBets_GetAllByBettor stored procedure which retrieves 
    /// all bets ever made by bettor
    /// </summary>
    /// <param name="id">
    /// int represents the id of the bettor that is the creater of the bets being retrieved from the database
    /// </param>
    /// <returns>IEnumerable of BetModel representing all bets created by bettor</returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorBets(int bettorId)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllByBettor", new
            {
                BettorId = bettorId
            });
    }

    /// <summary>
    /// Async method calls spBets_GetAllByGame stored procedure which retrieves 
    /// all bets placed on a certain game
    /// </summary>
    /// <param name="gameId">int id of the bettor that created the bets being retrieved </param>
    /// <returns>
    /// IEnumerable of BetModel representing all bets that were placed
    /// on a certain game
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetAllBetsOnGame(int gameId)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllByGame", new
            {
                GameId = gameId
            });
    }

    /// <summary>
    /// Async method calls the spBets_Insert stored procedure to insert one bet 
    /// entry into the database
    /// </summary>
    /// <param name="bet">BetModel represents a bet to insert into the database</param>
    /// <returns></returns>
    public async Task InsertBet(BetModel bet)
    {
        string betStatus = bet.BetStatus.ToString();
        string payoutStatus = bet.PayoutStatus.ToString();

        await _db.SaveData("dbo.spBets_Insert", new
        {
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
        }

        else
        {
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

    }

    /// <summary>
    /// Async method calls the spBets_Delete stored procedure which deletes one bet
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the id of the bet to be deleted from the database</param>
    /// <returns></returns>
    public async Task DeleteBet(int bettorId)
    {
        await _db.SaveData(
        "dbo.spBets_Delete", new
        {
            BettorId = bettorId
        });
    }
}
