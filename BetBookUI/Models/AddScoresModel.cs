using System.ComponentModel.DataAnnotations;

namespace BetBookUI.Models;

public class AddScoresModel
{
    [Required]
    [Range(1, 420, ErrorMessage = "Game Id cannot be less than 1")]
    public int GameId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Score cannot be less than zero")]
    public int FavoriteTeamScore { get; set; }

    [Required]
    [Range(0,int.MaxValue, ErrorMessage = "Score cannot be less than zero")]
    public int UnderdogTeamScore { get; set; }
}
