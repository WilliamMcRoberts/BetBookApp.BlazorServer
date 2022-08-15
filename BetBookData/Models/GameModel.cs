

namespace BetBookData.Models;

#nullable enable

/// <summary>
/// Game model
/// </summary>
public class GameModel
{
    // ID of game
    public int Id { get; set; }

    // Home team of the game
    public int HomeTeamId { get; set; }

    public TeamModel? HomeTeam { get; set; }

    // Away team of the game
    public int AwayTeamId { get; set; }

    public TeamModel? AwayTeam { get; set; }

    // Stadium the game is being played in
    public string Stadium { get; set; } = string.Empty;

    // The point spread of the game
    public double? PointSpread { get; set; }

    // Final score of the favorite
    public double HomeTeamFinalScore { get; set; }

    // Final score of the underdog
    public double AwayTeamFinalScore { get; set; }

    // Winner of the game
    public int? GameWinnerId { get; set; }

    public TeamModel? GameWinner { get; set; }

    // Week number
    public int WeekNumber { get; set; }

    // Season - PRE, REG, or POST
    public SeasonType Season { get; set; }

    // Date of the game
    public DateTime DateOfGame { get; set; }

    // Status of the game
    public GameStatus GameStatus { get; set; }

    public int ScoreId { get; set; }

    public string? PointSpreadDescription 
    { 
        get => 
            PointSpread < 0 ? 
            $"- {PointSpread?.ToString().Trim('-')} points" : 
            $"+ {PointSpread} points";
    }

    public string DateOfGameOnly { get; set; } = string.Empty;
    public string TimeOfGameOnly { get; set; } = string.Empty;
}


#nullable restore
