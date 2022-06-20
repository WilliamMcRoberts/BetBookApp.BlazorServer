using BetBookData.DataLogic.Interfaces;
using BetBookData.DbAccess;
using BetBookData.Models;

namespace BetBookData.DataLogic;

public class BetData : IBetData
{

    private readonly ISqlConnection _db;

    /// <summary>
    ///     BetData Constructor
    /// </summary>
    /// <param name="db">
    ///     ISqlConnection represents access to the database
    /// </param>
    public BetData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    ///     Async method calls the spBets_GetAll stored procedure to retrieve 
    ///     all bets in the database
    /// </summary>
    /// <returns>
    ///     IEnumerable of BetModel represents all bets in the database
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetBets()
    {
        return await _db.LoadData<BetModel, dynamic>(
        "dbo.spBets_GetAll", new { });
    }

    /// <summary>
    ///     Async method calls spBets_Get stored procedure which retrieves one 
    ///     bet by bet id
    /// </summary>
    /// <param name="id">
    ///     int represents the id of the bet being retrieved from the database
    /// </param>
    /// <returns>
    ///     BetModel represents the bet being retrieved from the database
    /// </returns>
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
    ///     Async method calls spBets_GetAllByBettor stored procedure which retrieves 
    ///     all bets ever made by bettor
    /// </summary>
    /// <param name="id">
    ///     int represents the id of the bettor that is the creater of the bets 
    ///     being retrieved from the database
    /// </param>
    /// <returns>
    ///     IEnumerable of BetModel representing all bets created by bettor
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    ///     Async method calls spBets_GetAllInProgressByBettor stored procedure  
    ///     which retrieves all bets with status "IN_PROGRESS" by bettor
    /// </summary>
    /// <param name="id">
    ///     int id of the bettor that created the bets being retrieved
    /// </param>
    /// <returns>
    ///     IEnumerable of BetModel representing all bets created by bettor with
    ///     the bet status of "IN_PROGRESS"
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorInProgressBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllInProgressByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    ///     Async method calls spBets_GetAllWinnersByBettor stored procedure which retrieves 
    ///     all bets that have a status of "WINNER" made by bettor
    /// </summary>
    /// <param name="id">
    ///     int id of the bettor that created the bets being retrieved 
    /// </param>
    /// <returns>
    ///     IEnumerable of BetModel representing all bets created by bettor with
    ///     the bet status of "WINNER"
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorWinningBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllWinnersByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    ///     Async method calls spBets_GetAllLosersByBettor stored procedure which retrieves 
    ///     all bets that have a status of "LOSER" made by bettor
    /// </summary>
    /// <param name="id">
    ///     int id of the bettor that created the bets being retrieved 
    /// </param>
    /// <returns>
    ///     IEnumerable of BetModel representing all bets created by bettor with
    ///     the bet status of "LOSER"
    /// </returns>
    public async Task<IEnumerable<BetModel>> GetAllBettorLosingBets(int id)
    {
        return await _db.LoadData<BetModel, dynamic>(
            "dbo.spBets_GetAllLosersByBettor", new
            {
                Id = id
            });
    }

    /// <summary>
    ///     Async method calls the spBets_Insert stored procedure to insert one bet 
    ///     entry into the database
    /// </summary>
    /// <param name="bet">
    ///     BetModel represents a bet to insert into the database
    /// </param>
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
    ///     Async method calls the spBets_Update stored procedure to update a bet
    /// </summary>
    /// <param name="bet">
    ///     BetModel represents a bet being updated into the database
    /// </param>
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
    ///     Async method calls the spBets_Delete stored procedure which deletes one bet
    ///     entry in the database
    /// </summary>
    /// <param name="id">
    ///     int represents the id of the bet to be deleted from the database
    /// </param>
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
