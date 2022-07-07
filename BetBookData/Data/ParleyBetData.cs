
using BetBookDbAccess;
using BetBookData.Models;

namespace BetBookData.Data;
public class ParleyBetData
{
    private readonly ISqlConnection _db;

    public ParleyBetData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spParleyBets_GetAll stored procedure to retrieve 
    /// all parley bets in the database
    /// </summary>
    /// <returns>IEnumerable of ParleyBetModel represents all parley bets in the database</returns>
    public async Task<IEnumerable<ParleyBetModel>> GetParleyBets()
    {
        return await _db.LoadData<ParleyBetModel, dynamic>(
        "dbo.spParleyBets_GetAll", new { });
    }

    /// <summary>
    /// Async method calls spParleyBets_Get stored procedure which retrieves one 
    /// parley bet by parley bet id
    /// </summary>
    /// <param name="parleyBetId">int represents the id of the bet being retrieved from the database</param>
    /// <returns>ParleyBetModel represents the parley bet being retrieved from the database</returns>
    public async Task<ParleyBetModel?> GetParleyBet(int parleyBetId)
    {
        var result = await _db.LoadData<ParleyBetModel, dynamic>(
            "dbo.spParleyBets_Get", new
            {
                Id = parleyBetId
            });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls spParleyBets_GetAllByBettor stored procedure which retrieves 
    /// all parley bets ever made by bettor
    /// </summary>
    /// <param name="bettorId">
    /// int represents the id of the bettor that is the creater of the parley bets being retrieved from the database
    /// </param>
    /// <returns>IEnumerable of ParleyBetModel representing all parley bets created by bettor</returns>
    public async Task<IEnumerable<ParleyBetModel>> GetAllBettorParleyBets(int bettorId)
    {
        return await _db.LoadData<ParleyBetModel, dynamic>(
            "dbo.spParleyBets_GetAllByBettor", new
            {
                BettorId = bettorId
            });
    }

    /// <summary>
    /// Async method calls the spParleyBets_Insert stored procedure to insert one parley bet 
    /// entry into the database
    /// </summary>
    /// <param name="parleyBet">ParleyBetModel represents a parley bet to insert into the database</param>
    /// <returns></returns>
    public async Task InsertParleyBet(ParleyBetModel parleyBet)
    {

        string parleyBetStatus = parleyBet.ParleyBetStatus.ToString();
        string parleyPayoutStatus = parleyBet.ParleyPayoutStatus.ToString();

        await _db.SaveData("dbo.spParleyBets_Insert", new
        {
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

    /// <summary>
    /// Async method calls the spParleyBets_Update stored procedure to update
    /// a parley bet
    /// </summary>
    /// <param name="parleyBet">ParleyBetModel represents a parley bet being updated into the database</param>
    /// <returns></returns>
    public async Task UpdateParleyBet(ParleyBetModel parleyBet)
    {
        string parleyBetStatus = parleyBet.ParleyBetStatus.ToString();
        string parleyPayoutStatus = parleyBet.ParleyPayoutStatus.ToString();

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
}
