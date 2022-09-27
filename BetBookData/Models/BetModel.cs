

namespace BetBookData.Models;

#nullable enable


public class BetModel
{
    public int Id { get; set; }

    public decimal BetAmount { get; set; }

    public decimal BetPayout { get; set; }

    public int BettorId { get; set; }

    public int GameId { get; set; }

    public GameModel? Game { get; set; }

    public int ChosenWinnerId { get; set; }

    public TeamModel? ChosenWinner { get; set; }

    public int? FinalWinnerId { get; set; }

    public TeamModel? FinalWinner { get; set; }

    public BetStatus BetStatus { get; set; }

    public PayoutStatus PayoutStatus { get; set; }

    public double? PointSpread { get; set; }
}

#nullable restore
