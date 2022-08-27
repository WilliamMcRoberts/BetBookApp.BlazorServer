
using BetBookData.Models;
using BetBookData.Interfaces;
using Microsoft.Extensions.Logging;
using BetBookDbAccess;

namespace BetBookData.Data;

#nullable enable


public class ParleyBetData : IParleyBetData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<ParleyBetData> _logger;

    public ParleyBetData(ISqlConnection db, ILogger<ParleyBetData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<ParleyBetModel>> GetParleyBets()
    {
        _logger.LogInformation(message: "Http Get / Get Parley Bets");

        return await _db.LoadData<ParleyBetModel, dynamic>(
            "dbo.spParleyBets_GetAll", new { });
    }

    public async Task<ParleyBetModel?> GetParleyBet(int parleyBetId)
    {
        _logger.LogInformation(message: "Http Get / Get Parley Bet");

        var result = await _db.LoadData<ParleyBetModel, dynamic>(
            "dbo.spParleyBets_Get", new
            {
                Id = parleyBetId
            });

        return result.FirstOrDefault();
    }

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



        string parleyBetStatus = ParleyBetStatus.IN_PROGRESS.ToStringFast();
        string parleyPayoutStatus = ParleyPayoutStatus.UNPAID.ToStringFast();

        _logger.LogInformation(message: "Http Post / Insert Parley Bet");

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

    public async Task UpdateParleyBet(ParleyBetModel parleyBet)
    {
        string parleyBetStatus = parleyBet.ParleyBetStatus.ToStringFast();
        string parleyPayoutStatus = parleyBet.ParleyPayoutStatus.ToStringFast();

        _logger.LogInformation(message: "Http Put / Update Parley Bet");

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

    public async Task DeleteParleyBet(int id)
    {
        _logger.LogInformation(message: "Http Delete / Delete Parley Bet");

        await _db.SaveData(
        "dbo.spBets_Delete", new
        {
            Id = id
        });
    }
}

#nullable restore
