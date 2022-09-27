
namespace BetBookData.Models;

#nullable enable

public class ParleyBetModel
{
    // Id of ParleyBetModel
    public int Id { get; set; }
    public int Bet1Id { get; set; }
    public int Bet2Id { get; set; }
    public int Bet3Id { get; set; }
    public int Bet4Id { get; set; }
    public int Bet5Id { get; set; }

    public int BettorId { get; set; }

    public decimal BetAmount { get; set; }

    public decimal BetPayout { get; set; }

    public ParleyBetStatus ParleyBetStatus { get; set; }

    public ParleyPayoutStatus ParleyPayoutStatus { get; set; }

    public List<BetModel> Bets { get; set; } = new();
}

#nullable restore
