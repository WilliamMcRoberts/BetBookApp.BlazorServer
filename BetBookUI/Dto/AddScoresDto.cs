using System.ComponentModel.DataAnnotations;

namespace BetBookUI.Dto;

public class AddScoresDto
{
    // Id of game 
    [Required]
    [Range(1, 420, ErrorMessage = "Game Id cannot be less than 1")]
    public int GameId { get; set; }

    // Score of the favorite
    [Required]
    [Range(0, 70, ErrorMessage = "Score cannot be less than zero")]
    public int FavoriteTeamScore { get; set; }

    // Score of the underdog
    [Required]
    [Range(0, 70, ErrorMessage = "Score cannot be less than zero")]
    public int UnderdogTeamScore { get; set; }
}
