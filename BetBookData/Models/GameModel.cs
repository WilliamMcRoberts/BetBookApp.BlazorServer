
namespace BetBookData.Models;

#nullable enable

public class GameModel
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public TeamModel? HomeTeam { get; set; }
    public int AwayTeamId { get; set; }
    public TeamModel? AwayTeam { get; set; }
    public string Stadium { get; set; } = string.Empty;
    public double? PointSpread { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public int? GameWinnerId { get; set; }
    public TeamModel? GameWinner { get; set; }
    public int WeekNumber { get; set; }
    public Season Season { get; set; }
    public DateTime DateOfGame { get; set; }
    public GameStatus GameStatus { get; set; }
    public int ScoreId { get; set; }
    public string DateOfGameOnly { get; set; } = string.Empty;
    public string TimeOfGameOnly { get; set; } = string.Empty;

    public string? PointSpreadDescription =>
        PointSpread < 0 ?
        $"- {PointSpread?.ToString().Trim('-')} points" 
        : $"+ {PointSpread} points";
}


#nullable restore
