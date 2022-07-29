
namespace BetBookData.Models;

/// <summary>
/// Team model
/// </summary>
public class TeamModel
{
    // ID of the team
    public int Id { get; set; }

    

    // Name of the team
    public string TeamName { get; set; }

    // City of the team
    public string City { get; set; }

    // Stadium of the team
    public string Stadium { get; set; }

    // Teams that the current team have beaten
    public string Wins { get; set; }

    // Teams that the current team has lost to
    public string Losses { get; set; }

    // Teams that the current team have been in a draw with
    public string Draws { get; set; }

    public string Symbol { get; set; }


    public string ImagePath { get => $"{TeamName.ToLower()}.svg"; }

}
