using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public class BetData : IBetData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// Contstructor for BetData class
    /// </summary>
    /// <param name="db"></param>
    public BetData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Method calls the spBets_GetAll stored procedure to retrieve 
    /// all bets in the database
    /// </summary>
    /// <returns>IEnumerable of BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetBets()
    {
        return await _db.LoadData<BetModel, dynamic>(
        "dbo.spBets_GetAll", new { });
    }

    /// <summary>
    /// Method calls spBets_Get stored procedure which retrieves one 
    /// bet by bet id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<BetModel?> GetBet(int id)
    {
        var result = await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_Get", new
            {
                Id = id
            });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Method calls spBets_GetAllByBetter stored procedure which retrieves 
    /// all bets ever made by better
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBetterBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllByBetter", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllInProgressByBetter stored procedure which retrieves 
    /// all open bets by better
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBetterInProgressBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllInProgressByBetter", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllWinnersByBetter stored procedure which retrieves 
    /// all bets that have a status of "WINNER" made by better
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBetterWinningBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllWinnersByBetter", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllLosersByBetter stored procedure which retrieves 
    /// all bets that have a "LOSER" status made by better
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBetterLosingBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllLosersByBetter", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls the spBets_Insert stored procedure to insert one bet 
    /// entry into the database
    /// </summary>
    /// <param name="bet"></param>
    /// <returns></returns>
    public async Task InsertBet(BetModel bet)
    {
        var bettorId = bet.Bettor.Id;
        var gameId = bet.GameInBet.Id;
        var chosenWinnerId = bet.ChosenWinner.Id;
        var betStatus = bet.BetStatus.ToString();

        await _db.SaveData("dbo.spBets_Insert", new
        {
            bet.BetAmount,
            bet.BetPayout,
            bettorId,
            gameId,
            chosenWinnerId,
            betStatus
        });
    }

    /// <summary>
    /// Method calls the spBets_Update stored procedure to update a bet
    /// </summary>
    /// <param name="bet"></param>
    /// <returns></returns>
    public async Task UpdateBet(BetModel bet)
    {
        var bettorId = bet.Bettor.Id;
        var gameId = bet.GameInBet.Id;
        var chosenWinnerId = bet.ChosenWinner.Id;
        var finalWinnerId = bet.FinalWinner.Id;
        var betStatus = bet.BetStatus.ToString();

        await _db.SaveData("dbo.spBets_Update", new
        {
            bet.Id,
            bet.BetAmount,
            bet.BetPayout,
            bettorId,
            gameId,
            chosenWinnerId,
            finalWinnerId,
            betStatus
        });
    }

    /// <summary>
    /// Method calls the spBets_Delete stored procedure which deletes one bet
    /// entry in the database
    /// </summary>
    /// <param name="id"></param>
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
