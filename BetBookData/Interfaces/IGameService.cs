using BetBookData.Lookups;
using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IGameService
{
    Task FetchAllScoresForFinishedGames();
    Task<GameByScoreIdDto> GetGameByScoreId(int scoreId);
    Task<GameByTeamDto> GetGameByTeam(TeamModel team);
    Task<Game[]> GetGamesByWeek(SeasonType currentSeason, int week);
    Task<HashSet<GameModel>> GetGamesForThisWeek(SeasonType currentSeason, int currentWeek);
    Task GetPointSpreadUpdateForAvailableGames();
}