
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

    // Count of total season wins
    public int WinCount { get; set; }

    // Count of total season losses
    public int LossCount { get; set; }

    // Count of total season draw
    public int Drawcount { get; set; }
   
}
