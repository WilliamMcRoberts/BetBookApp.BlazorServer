
namespace BetBookData.Models;

#nullable enable


public class TeamModel
{
    public int Id { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Stadium { get; set; } = string.Empty;

    public string Wins { get; set; } = string.Empty;

    public string Losses { get; set; } = string.Empty;

    public string Draws { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public string Division { get; set; } = string.Empty;

    public string Conference { get; set; } = string.Empty;

    public string ImagePath
    {
        get => TeamName == "Ravens" ? $"{TeamName.ToLower()}.png" 
               : $"{TeamName.ToLower()}.svg";
    }

    public string[] TeamWins 
    { 
        get => Wins.Split('|').SkipLast(1).ToArray(); 
    }

    public string[] TeamLosses 
    {
        get => Losses.Split('|').SkipLast(1).ToArray(); 
    }

    public string[] TeamDraws 
    { 
        get => Draws.Split('|').SkipLast(1).ToArray();
    }
}

#nullable restore
