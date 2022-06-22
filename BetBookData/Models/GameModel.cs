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
    // Away team of the game
    public int AwayTeamId { get; set; }
    // Team that is declared the favorite
    public int FavoriteId { get; set; }
    // Team that is declared the underdog
    public int UnderdogId { get; set; }
    // Stadium the game is being played in
    public string Stadium { get; set; }
    // TODO - Change data type of point spread
    // The point spread of the game
    public int PointSpread { get; set; }
    // Final score of the favorite
    public int FavoriteFinalScore { get; set; }
    // Final score of the underdog
    public int UnderdogFinalScore { get; set; }
    // Winner of the game
    public int GameWinnerId { get; set; }
    // Week number
    public int Week { get; set; }
    // Season - PRE, REG, or POST
    public SeasonType Season { get; set; }
    // Date of the game
    public DateTime? DateOfGame { get; set; }
    // Status of the game
    public GameStatus GameStatus { get; set; }



}
