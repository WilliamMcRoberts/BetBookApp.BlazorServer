

namespace BetBookData.Models;

#nullable enable

/// <summary>
/// Bet model
/// </summary>
public class BetModel
{
    // ID of bet
    public int Id { get; set; }

    // Amount of the wager placed on bet
    public decimal BetAmount { get; set; }

    // Payout of bet if bet is won
    public decimal BetPayout { get; set; }

    // User that initiated the bet
    public int BettorId { get; set; }

    // Id of game in bet
    public int GameId { get; set; }

    // Game in bet
    public GameModel? Game { get; set; }

    // Chosen winner of the game in the bet
    public int ChosenWinnerId { get; set; }

    // Team name of the chosen winner
    public TeamModel? ChosenWinner { get; set; }

    // Final winner of the game in the bet
    public int? FinalWinnerId { get; set; }

    // Team name of the final winner
    public TeamModel? FinalWinner { get; set; }

    // Status of the bet
    public BetStatus BetStatus { get; set; }

    // Status of the payout of the bet
    public PayoutStatus PayoutStatus { get; set; }

    public double? PointSpread { get; set; }
}

#nullable restore
