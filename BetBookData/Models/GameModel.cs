using System.ComponentModel.DataAnnotations;
using BetBookData;

namespace BetBookData.Models;

/// <summary>
/// Game model
/// </summary>
public class GameModel
{
    // ID of game
    public int Id { get; set; }

    // Home team of the game
    public int HomeTeamId { get; set; }

    public TeamModel HomeTeam { get; set; }

    // Away team of the game
    public int AwayTeamId { get; set; }

    public TeamModel AwayTeam { get; set; }

    // Team that is declared the favorite
    public int FavoriteId { get; set; }

    public TeamModel Favorite { get; set; }

    // Team that is declared the underdog
    public int UnderdogId { get; set; }

    public TeamModel Underdog { get; set; }

    // Stadium the game is being played in
    public string Stadium { get; set; }

    // The point spread of the game
    public double PointSpread { get; set; }

    // Final score of the favorite
    public double HomeTeamFinalScore { get; set; }

    // Final score of the underdog
    public double AwayTeamFinalScore { get; set; }

    // Winner of the game
    public int GameWinnerId { get; set; }

    public TeamModel GameWinner { get; set; }

    // Week number
    public int WeekNumber { get; set; }

    // Season - PRE, REG, or POST
    public SeasonType Season { get; set; }

    // Date of the game
    public DateTime DateOfGame { get; set; }

    // Status of the game
    public GameStatus GameStatus { get; set; }

    public int ScoreId { get; set; }

    public string PointSpreadDescription { get => (Math.Round(PointSpread) < 0) ? $"- {PointSpread.ToString().Trim('-')} points" : $"+ {PointSpread} points"; }

    public string DateOfGameOnly { get; set; }
    public string TimeOfGameOnly { get; set; }
}
