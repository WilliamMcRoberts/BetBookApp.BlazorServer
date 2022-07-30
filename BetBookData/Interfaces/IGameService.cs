using BetBookData.Lookups;
using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IGameService
{
    Task FetchAllScoresForFinishedGames();
    Task<GameLookup> GetGameByScoreIdLookup(int scoreId);
    Task<TeamLookup> GetGameByTeamLookup(TeamModel team);
    Task<Game[]> GetGamesByWeekLookup(SeasonType currentSeason, int week);
    Task<List<GameModel>> GetGamesForNextWeek(SeasonType currentSeason, int currentWeek);
    Task GetPointSpreadUpdateForAvailableGames();
}