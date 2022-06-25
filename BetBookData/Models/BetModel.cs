
using BetBookData;

namespace BetBookData.Models;

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
    // Game that is being bet on
    public int GameId { get; set; }
    // Chosen winner of the game in the bet
    public int ChosenWinnerId { get; set; }
    // Final winner of the game in the bet
    public int FinalWinnerId { get; set; }
    // Status of the bet
    public BetStatus BetStatus { get; set; }
    // Status of the payout of the bet
    public PayoutStatus PayoutStatus { get; set; }



}
