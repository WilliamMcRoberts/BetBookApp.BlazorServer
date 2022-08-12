
namespace BetBookData.Models;

#nullable enable


/// <summary>
/// Team model
/// </summary>
public class TeamModel
{
    // ID of the team
    public int Id { get; set; }

    // Name of the team
    public string TeamName { get; set; } = string.Empty;

    // City of the team
    public string City { get; set; } = string.Empty;

    // Stadium of the team
    public string Stadium { get; set; } = string.Empty;

    // Teams that the current team have beaten
    public string Wins { get; set; } = string.Empty;


    // Teams that the current team has lost to
    public string Losses { get; set; } = string.Empty;


    // Teams that the current team have been in a draw with
    public string Draws { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public string Division { get; set; } = string.Empty;

    public string Conference { get; set; } = string.Empty;

    public string ImagePath 
    { 
        get => TeamName == "Ravens" ? $"{TeamName.ToLower()}.png" : $"{TeamName.ToLower()}.svg"; 
    }

    public string[] TeamWins { get => Wins.Split('|').SkipLast(1).ToArray(); }

    public string[] TeamLosses { get => Losses.Split('|').SkipLast(1).ToArray(); }

    public string[] TeamDraws { get => Draws.Split('|').SkipLast(1).ToArray(); }

}

#nullable restore
