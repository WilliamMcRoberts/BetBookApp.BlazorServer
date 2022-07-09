namespace BetBookData.Models;

public class BasicBetModel
{
    // Team name of the chosen winner
    public string ChosenWinnerTeamName { get; set; }

    // Team name of the final winner
    public string? FinalWinnerTeamName { get; set; }

    // Point spread of the game in the bet
    public double Spread { get; set; }

    // Bet payout amount
    public decimal PayoutAmount { get; set; }

    
}
