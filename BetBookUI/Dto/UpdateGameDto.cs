using System.ComponentModel.DataAnnotations;

namespace BetBookUI.Dto;

public class UpdateGameDto
{
    [Required]
    public int GameId { get; set; }

    // Home team Id
    [Required]
    [Range(1, 32, ErrorMessage = "Home Team Id must be in the range of 1 - 32")]
    public int HomeTeamId { get; set; }

    // Away team Id 
    [Required]
    [Range(1, 32, ErrorMessage = "Away Team Id must be in the range of 1 - 32")]
    public int AwayTeamId { get; set; }

    // Id of team that is declared the favorite
    [Required]
    [Range(1, 32, ErrorMessage = "Favorite Team Id must be in the range of 1 - 32")]
    public int FavoriteId { get; set; }

    // Id of team that is declared the underdog
    [Required]
    [Range(1, 32, ErrorMessage = "Underdog Team Id must be in the range of 1 - 32")]
    public int UnderdogId { get; set; }

    // Stadium the game is being played in
    [Required]
    [MinLength(7, ErrorMessage = "Stadium is required and must be more than 7 characters")]
    public string Stadium { get; set; }

    // The point spread of the game
    [Required]
    public string PointSpread { get; set; }

    // Date of the game
    [Required]
    public DateTime DateOfGame { get; set; }

    public double HomeTeamFinalScore { get; set; }

    public double AwayTeamFinalScore { get; set; }

    public int GameWinnerId { get; set; }

    [Required]
    [Range(0, 17, ErrorMessage = "Week number must be from 0 to 17")]
    public int Week { get; set; }

    [Required]
    public string Season { get; set; }

    [Required]
    public string GameStatus { get; set; }

    [Required]
    public int ScoreId { get; set; }
}
