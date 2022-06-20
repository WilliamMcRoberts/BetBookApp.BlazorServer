
namespace BetBookData.Models;
/// <summary>
/// Team record model
/// </summary>
public class TeamRecordModel
{
    // Id for record entry
    public int Id { get; set; }
    // Id of the team
    public int TeamId { get; set; }
    // Teams that the current team have beaten
    public string Wins { get; set; }
    // Teams that the current team has lost to
    public string Losses { get; set; }
    // Teams that the current team have been in a draw with
    public string Draws { get; set; }
}
