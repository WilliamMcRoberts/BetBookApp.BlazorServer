using BetBookData.DataLogic.Interfaces;
using BetBookData.DbAccess;
using BetBookData.Models;

namespace BetBookData.DataLogic;

public class BetData : IBetData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// BetData Constructor
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
    /// Method calls spBets_GetAllByBettor stored procedure which retrieves 
    /// all bets ever made by bettor
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllInProgressByBettor stored procedure which retrieves 
    /// all open bets by bettor
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorInProgressBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllInProgressByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllWinnersByBettor stored procedure which retrieves 
    /// all bets that have a status of "WINNER" made by bettor
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorWinningBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllWinnersByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    /// Method calls spBets_GetAllLosersByBettor stored procedure which retrieves 
    /// all bets that have a "LOSER" status made by better
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BetModel</returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorLosingBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllLosersByBettor", new
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
        int bettorId = bet.Bettor.Id;
        int gameId = bet.GameInBet.Id;
        int chosenWinnerId = bet.ChosenWinner.Id;
        string betStatus = bet.BetStatus.ToString();
        string payoutStatus = bet.PayoutStatus.ToString();

        await _db.SaveData("dbo.spBets_Insert", new
        {
            bet.BetAmount,
            bet.BetPayout,
            bettorId,
            gameId,
            chosenWinnerId,
            betStatus,
            payoutStatus
        });
    }

    /// <summary>
    /// Method calls the spBets_Update stored procedure to update a bet
    /// </summary>
    /// <param name="bet"></param>
    /// <returns></returns>
    public async Task UpdateBet(BetModel bet)
    {
        int bettorId = bet.Bettor.Id;
        int gameId = bet.GameInBet.Id;
        int chosenWinnerId = bet.ChosenWinner.Id;
        int finalWinnerId = bet.FinalWinner.Id;
        string betStatus = bet.BetStatus.ToString();
        string payoutStatus = bet.PayoutStatus.ToString();

        await _db.SaveData("dbo.spBets_Update", new
        {
            bet.Id,
            bet.BetAmount,
            bet.BetPayout,
            bettorId,
            gameId,
            chosenWinnerId,
            finalWinnerId,
            betStatus,
            payoutStatus
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
