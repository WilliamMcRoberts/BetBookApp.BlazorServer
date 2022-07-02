namespace BetBookUI.Helpers;

public static class GamePopulationHelpers
{
    /// <summary>
    /// Async method opulates a list of basic game models
    /// </summary>
    /// <param name="games">List<GameModel> represents a list of games to use for populating basic game list</param>
    /// <returns></returns>
    public static async Task<List<BasicGameModel>> PopulateBasicGameModelList(
                List<GameModel> games, IGameData gameData, ITeamData teamData)
    {
        List<BasicGameModel> basicGames = new();

        foreach (GameModel g in games)
        {
            // If game has started update game status and re-populate basic games
            if (g.DateOfGame < DateTime.Now)
            {
                g.GameStatus = GameStatus.IN_PROGRESS;
                await gameData.UpdateGame(g);
                games.Remove(g);
                await PopulateBasicGameModelList(games, gameData, teamData);
            }

            TeamModel? homeTeam = await teamData.GetTeam(g.HomeTeamId);
            TeamModel? awayTeam = await teamData.GetTeam(g.AwayTeamId);
            TeamModel? favoriteTeam = await teamData.GetTeam(g.FavoriteId);
            TeamModel? underdogTeam = await teamData.GetTeam(g.UnderdogId);

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
    public static async Task<TeamRecordModel[]> GetTeamRecords(
            List<BasicGameModel> basicGames, IGameData gameData, 
            ITeamData teamData, ITeamRecordData recordData)
    {
        TeamRecordModel[] teamRecords = new TeamRecordModel[32];
        int index = 0;

        foreach (BasicGameModel bg in basicGames)
        {
            GameModel? game = await gameData.GetGame(bg.GameId);

            if (game is not null)
            {
                TeamModel? teamHome = await teamData.GetTeam(game.HomeTeamId);
                TeamModel? teamAway = await teamData.GetTeam(game.AwayTeamId);

                if (teamHome is not null && teamAway is not null)
                {
                    TeamRecordModel? teamRecordHome =
                        await recordData.GetTeamRecord(teamHome.Id);
                    TeamRecordModel? teamRecordAway =
                        await recordData.GetTeamRecord(teamAway.Id);

                    if (teamRecordHome is not null && teamRecordAway is not null)
                    {
                        teamRecords[index] = teamRecordAway;
                        teamRecords[index + 1] = teamRecordHome;
                    }
                }

                index += 2;
            }
        }

        return teamRecords;
    }
}
