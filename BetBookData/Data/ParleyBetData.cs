
using BetBookData.Models;
using BetBookData.Interfaces;
using BetBookData.DbAccess;

namespace BetBookData.Data;
public class ParleyBetData : IParleyBetData
{
    private readonly ISqlConnection _db;
    private readonly IBetData _betData;

    public ParleyBetData(ISqlConnection db, IBetData betData)
    {
        _db = db;
        _betData = betData;
    }

    /// <summary>
    /// Async method calls the spParleyBets_GetAll stored procedure to retrieve 
    /// all parley bets in the database
    /// </summary>
    /// <returns>IEnumerable of ParleyBetModel represents all parley bets in the database</returns>
    public async Task<IEnumerable<ParleyBetModel>> GetParleyBets() => 
        await _db.LoadData<ParleyBetModel, dynamic>( "dbo.spParleyBets_GetAll", new { });

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
    /// Async method calls the spParleyBets_Insert stored procedure to insert one parley bet 
    /// entry into the database
    /// </summary>
    /// <param name="parleyBet">ParleyBetModel represents a parley bet to insert into the database</param>
    /// <returns></returns>
    public async Task InsertParleyBet(ParleyBetModel parleyBet)
    {

        parleyBet.Bet1Id = parleyBet.Bets[0].Id;
        parleyBet.Bet2Id = parleyBet.Bets[1].Id;

        if (parleyBet.Bets.Count > 2)
        {
            parleyBet.Bet3Id = parleyBet.Bets[2].Id;
        }
        else
            parleyBet.Bet3Id = 0;

        if (parleyBet.Bets.Count > 3)
        {
            parleyBet.Bet4Id = parleyBet.Bets[3].Id;
        }
        else
            parleyBet.Bet4Id = 0;

        if (parleyBet.Bets.Count > 4)
        {
            parleyBet.Bet5Id = parleyBet.Bets[4].Id;
        }
        else
            parleyBet.Bet5Id = 0;



        string parleyBetStatus = ParleyBetStatus.IN_PROGRESS.ToString();
        string parleyPayoutStatus = ParleyPayoutStatus.UNPAID.ToString();

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

    /// <summary>
    /// Async method calls the spBets_Delete stored procedure which deletes one bet
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the id of the bet to be deleted from the database</param>
    /// <returns></returns>
    public async Task DeleteParleyBet(int id)
    {
        await _db.SaveData(
        "dbo.spBets_Delete", new
        {
            Id = id
        });
    }
}
