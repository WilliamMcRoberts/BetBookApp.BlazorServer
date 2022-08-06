using BetBookData.Lookups;
using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IGameService
{
    Task FetchAllScoresForFinishedGames();
    Task<GameByScoreIdLookup> GetGameByScoreIdLookup(int scoreId);
    Task<GameByTeamLookup> GetGameByTeamLookup(TeamModel team);
    Task<Game[]> GetGamesByWeekLookup(SeasonType currentSeason, int week);
    Task<List<GameModel>> GetGamesForNextWeek(SeasonType currentSeason, int currentWeek);
    Task GetPointSpreadUpdateForAvailableGames();
}