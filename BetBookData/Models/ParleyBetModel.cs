
namespace BetBookData.Models;
public class ParleyBetModel
{
    // Id of ParleyBetModel
    public int Id { get; set; }

    public List<BetModel> Bets { get; set; } = new();

    public int Bet1Id { get; set; }
    public int Bet2Id { get; set; }
    public int Bet3Id { get; set; }
    public int Bet4Id { get; set; }
    public int Bet5Id { get; set; }
    // Id of the bettor
    public int BettorId { get; set; }

    // Amount being wagered on parley bet
    public decimal BetAmount { get; set; }

    // Payout amount if parley bet is won
    public decimal BetPayout { get; set; }

    // Status of the parley bet
    public ParleyBetStatus ParleyBetStatus { get; set; }

    // Payout status of the parley bet
    public ParleyPayoutStatus ParleyPayoutStatus { get; set; }
}
