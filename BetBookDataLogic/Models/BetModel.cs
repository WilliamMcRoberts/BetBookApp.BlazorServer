
using BetBookDataLogic;

namespace BetBookDataLogic.Models;

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
    public UserModel Bettor { get; set; }
    // Game that is being bet on
    public GameModel GameInBet { get; set; }
    // Chosen winner of the game in the bet
    public TeamModel ChosenWinner { get; set; }
    // Final winner of the game in the bet
    public TeamModel FinalWinner { get; set; }
    // Status of the bet
    public BetStatus BetStatus { get; set; }
    // Status of the payout of the bet
    public PayoutStatus PayoutStatus { get; set; }



}
