namespace BetBookUI.Helpers;

public static class PopulationHelpers
{
    /// <summary>
    /// Async method populates a list of basic game models
    /// </summary>
    /// <param name="games">List<GameModel> represents a list of games to use for populating basic game list</param>
    /// <returns></returns>
    public static async Task<List<BasicGameModel>> PopulateBasicGameModelListFromGameList(
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
                await PopulateBasicGameModelListFromGameList(games, gameData, teamData);
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
    public static async Task<TeamRecordModel[]> PopulateTeamRecordsArrayFromBasicGameList(
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

    /// <summary>
    /// Async method popluates a basic game model with the team names and 
    /// point spread of current selected game
    /// </summary>
    /// <param name="game">GameModel representing the current game</param>
    /// <returns></returns>
    public static async Task<BasicGameModel> PopulateBasicGameModelFromGame(
        GameModel game, ITeamData teamData)
    {
        TeamModel? homeTeam = await teamData.GetTeam(game.HomeTeamId);
        TeamModel? awayTeam = await teamData.GetTeam(game.AwayTeamId);
        TeamModel? favorite = await teamData.GetTeam(game.FavoriteId);
        TeamModel? underdog = await teamData.GetTeam(game.UnderdogId);

        BasicGameModel basicGame = new();

        if (homeTeam is not null && awayTeam is not null &&
            favorite is not null && underdog is not null)
        {

            basicGame.HomeTeamName = homeTeam.TeamName;
            basicGame.AwayTeamName = awayTeam.TeamName;
            basicGame.FavoriteTeamName = favorite.TeamName;
            basicGame.UnderdogTeamName = underdog.TeamName;
            basicGame.PointSpread = game.PointSpread;
        }

        return basicGame;
    }

    /// <summary>
    /// Async method populates and returns a list of records lists from current game
    /// </summary>
    /// <param name="game">
    /// List<List<string>> Represents list of records lists from current game
    /// </param>
    /// <returns>List<List<string>> Represents a list of records lists</returns>
    public static async Task<List<List<string>>> PopulateRecordsListsFromGame(
        GameModel game, ITeamRecordData recordData)
    {
        TeamRecordModel? favRecord =
            await recordData.GetTeamRecord(game.FavoriteId);
        TeamRecordModel? undRecord =
            await recordData.GetTeamRecord(game.UnderdogId);

        List<List<string>> recordsLists = new();

        List<string> favoriteTeamWins = new();
        List<string> favoriteTeamLosses = new();
        List<string> favoriteTeamDraws = new();
        List<string> underdogTeamWins = new();
        List<string> underdogTeamLosses = new();
        List<string> underdogTeamDraws = new();

        if (favRecord is not null)
        {
            favoriteTeamWins = favRecord.Wins.Split('|').ToList();
            favoriteTeamWins.RemoveRange(favoriteTeamWins.Count - 1, 1);
            recordsLists.Add(favoriteTeamWins);

            favoriteTeamLosses = favRecord.Losses.Split('|').ToList();
            favoriteTeamLosses.RemoveRange(favoriteTeamLosses.Count - 1, 1);
            recordsLists.Add(favoriteTeamLosses);

            favoriteTeamDraws = favRecord.Draws.Split('|').ToList();
            favoriteTeamDraws.RemoveRange(favoriteTeamDraws.Count - 1, 1);
            recordsLists.Add(favoriteTeamDraws);
        }

        if (undRecord is not null)
        {
            underdogTeamWins = undRecord.Wins.Split('|').ToList();
            underdogTeamWins.RemoveRange(underdogTeamWins.Count - 1, 1);
            recordsLists.Add(underdogTeamWins);

            underdogTeamLosses = undRecord.Losses.Split('|').ToList();
            underdogTeamLosses.RemoveRange(underdogTeamLosses.Count - 1, 1);
            recordsLists.Add(underdogTeamLosses);

            underdogTeamDraws = undRecord.Draws.Split('|').ToList();
            underdogTeamDraws.RemoveRange(underdogTeamDraws.Count - 1, 1);
            recordsLists.Add(underdogTeamDraws);
        }

        return recordsLists;
    }

    /// <summary>
    /// Method populates team stats in game
    /// </summary>
    /// <param name="recordsLists">List<List<string>> Represents the list of records lists to 
    /// sort into proper team stats
    /// </param>
    /// <returns>int[] Represents the stats of both teams in current game</returns>
    public static int[] PopulateTeamStatsFromRecordLists(
        List<List<string>> recordsLists)
    {
        int[] stats = new int[6];

        stats[0] = recordsLists[0].Count;
        stats[1] = recordsLists[1].Count;
        stats[2] = recordsLists[2].Count;
        stats[3] = recordsLists[3].Count;
        stats[4] = recordsLists[4].Count;
        stats[5] = recordsLists[5].Count;

        return stats;
    }

    /// <summary>
    /// Async static method populates a list of basic bets from a list of bets
    /// </summary>
    /// <param name="bets">List<BetModel></param>
    /// <param name="gameData">IGameData</param>
    /// <param name="teamData">ITeamData</param>
    /// <returns>List<BasicBetModel></returns>
    public static async Task<List<BasicBetModel>> PopulateBasicBetsListFromBetsList(
        List<BetModel> bets, IGameData gameData, ITeamData teamData)
    {
        List<BasicBetModel> basicBetsList = new();

        foreach (BetModel bet in bets)
        {
            BasicBetModel bb = new();
            
            TeamModel? chosenTeam = await teamData.GetTeam(bet.ChosenWinnerId);
            GameModel? gameWagered = await gameData.GetGame(bet.GameId);

            if (chosenTeam is not null && gameWagered is not null)
            {
                bb.ChosenWinnerTeamName = chosenTeam.TeamName;
                bb.FinalWinnerTeamName = "Game Not Finished";
                bb.Spread = gameWagered.PointSpread;
                bb.PayoutAmount =
                    Convert.ToDecimal((
                        bet.BetAmount + bet.BetPayout).ToString("#.00"));
                basicBetsList.Add(bb);
            }
        }

        return basicBetsList;
    }

    /// <summary>
    /// Async static method to populate basic game model
    /// for current game being updated
    /// </summary>
    /// <param name="gameId">int Id of current game</param>
    /// <returns></returns>
    public static async Task<BasicGameModel> PopulateBasicGameModelFromGameId(
        int gameId, IGameData gameData, ITeamData teamData)
    {
        GameModel currentGame = new();
        BasicGameModel basicGame = new();

        currentGame = await gameData.GetGame(gameId);

        if (currentGame is not null)
        {
            TeamModel? currentHomeTeam = new();
            TeamModel? currentAwayTeam = new();
            TeamModel? currentFavoriteTeam = new();
            TeamModel? currentUnderdogTeam = new();

            currentHomeTeam = await teamData.GetTeam(currentGame.HomeTeamId);
            currentAwayTeam = await teamData.GetTeam(currentGame.AwayTeamId);
            currentFavoriteTeam = await teamData.GetTeam(currentGame.FavoriteId);
            currentUnderdogTeam = await teamData.GetTeam(currentGame.UnderdogId);

            basicGame.HomeTeamName = currentHomeTeam.TeamName;
            basicGame.AwayTeamName = currentAwayTeam.TeamName;
            basicGame.FavoriteTeamName = currentFavoriteTeam.TeamName;
            basicGame.UnderdogTeamName = currentUnderdogTeam.TeamName;
            basicGame.PointSpread = currentGame.PointSpread;
        }

        return basicGame;
    }
}
