using BetBookData.DataLogic.Interfaces;
using BetBookData.Models;

namespace BetBookData.DataLogic;
public class AvailableGames : IAvailableGames
{
    private readonly IGameData _gameData;
    private readonly ITeamData _teamData;
    private readonly ITeamRecordData _recordData;

    /// <summary>
    /// AvailableGames constructor
    /// </summary>
    /// <param name="gameData">IGameData represents game data interface</param>
    /// <param name="teamData">ITeamData represents team data interface</param>
    /// <param name="recordData">IRecordData represents team record data interface</param>
    public AvailableGames(IGameData gameData, ITeamData teamData, ITeamRecordData recordData)
    {
        _gameData = gameData;
        _teamData = teamData;
        _recordData = recordData;
    }

    /// <summary>
    /// Async method opulates a list of basic game models
    /// </summary>
    /// <param name="games">List<GameModel> represents a list of games to use for populating basic game list</param>
    /// <returns></returns>
    public async Task<List<BasicGameModel>> PopulateBasicGameModelList(List<GameModel> games)
    {

        List<BasicGameModel> basicGames = new();



        foreach (GameModel g in games)
        {
            // If game has started update game status and re-populate basic games
            if (g.DateOfGame < DateTime.Now)
            {
                g.GameStatus = GameStatus.IN_PROGRESS;
                await _gameData.UpdateGame(g);
                games.Remove(g);
                await PopulateBasicGameModelList(games);
            }

            TeamModel? homeTeam = await _teamData.GetTeam(g.HomeTeamId);
            TeamModel? awayTeam = await _teamData.GetTeam(g.AwayTeamId);
            TeamModel? favoriteTeam = await _teamData.GetTeam(g.FavoriteId);
            TeamModel? underdogTeam = await _teamData.GetTeam(g.UnderdogId);

            BasicGameModel bg = new();

            if (homeTeam is not null && awayTeam is not null
                && favoriteTeam is not null && underdogTeam is not null)
            {
                bg.HomeTeamName = homeTeam.TeamName;
                bg.AwayTeamName = awayTeam.TeamName;
                bg.FavoriteTeamName = favoriteTeam.TeamName;
                bg.UnderdogTeamName = underdogTeam.TeamName;
                bg.PointSpread = g.PointSpread;
                bg.GameId = g.Id;

                basicGames.Add(bg);
            }
        }

        return basicGames;
    }

    /// <summary>
    /// Async method populates an array of team records
    /// </summary>
    /// <param name="basicGames">
    /// List<BasicGameModel> represents a list of basic games
    /// to use populate the team record array
    /// </param>
    /// <returns>TeamRecordModel[] array of team records</returns>
    public async Task<TeamRecordModel[]> GetTeamRecords(List<BasicGameModel> basicGames)
    {
        TeamRecordModel[] teamRecords = new TeamRecordModel[32];
        int index = 0;

        foreach (BasicGameModel bg in basicGames)
        {
            GameModel? game = await _gameData.GetGame(bg.GameId);

            TeamModel? teamHome = await _teamData.GetTeam(game.HomeTeamId);
            TeamModel? teamAway = await _teamData.GetTeam(game.AwayTeamId);

            if (teamHome is not null && teamAway is not null)
            {
                TeamRecordModel? teamRecordHome = await _recordData.GetTeamRecord(teamHome.Id);
                TeamRecordModel? teamRecordAway = await _recordData.GetTeamRecord(teamAway.Id);

                if (teamRecordHome is not null && teamRecordAway is not null)
                {
                    teamRecords[index] = teamRecordAway;
                    teamRecords[index + 1] = teamRecordHome;
                }
            }

            index += 2;
        }

        return teamRecords;
    }
}
