using BetBookData.Lookups;
using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IGameService
{
    Task FetchAllScoresForFinishedGames();
    Task<GameByScoreIdDto> GetGameByScoreIdLookup(int scoreId);
    Task<GameByTeamDto> GetGameByTeamLookup(TeamModel team);
    Task<Game[]> GetGamesByWeekLookup(SeasonType currentSeason, int week);
    Task<HashSet<GameModel>> GetGamesForThisWeek(SeasonType currentSeason, int currentWeek);
    Task GetPointSpreadUpdateForAvailableGames();
}