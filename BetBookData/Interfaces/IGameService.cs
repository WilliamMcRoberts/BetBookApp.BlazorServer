using BetBookData.Dto;

namespace BetBookData.Interfaces;

public interface IGameService
{
    Task<GameByScoreIdDto> GetGameByScoreId(int scoreId);
    Task<GameDto[]> GetGameDtoArrayByWeekAndSeason(int _week, Season _season);
}