namespace BetBookUI.Models;

public class BasicBetModel
{
    public string ChosenWinnerTeamName { get; set; }
    public string? FinalWinnerTeamName { get; set; }
    public double Spread { get; set; }
    public decimal PayoutAmount { get; set; }
}
