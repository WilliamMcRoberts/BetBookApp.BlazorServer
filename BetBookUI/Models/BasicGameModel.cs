
namespace BetBookUI.Models;
public class BasicGameModel
{
    // Id of the game of the basic game model
    public int GameId { get; set; }

    // Name of the home team
    public string HomeTeamName { get; set; }

    // Name of the away team
    public string AwayTeamName { get; set; }

    // Name of the favorited team
    public string FavoriteTeamName { get; set; }

    // Name of the underdog team
    public string UnderdogTeamName { get; set; }

    // Point spread of the game
    public double PointSpread { get; set; }
}
