namespace BetBookUI.Helpers;

public static class CalculationHelpers
{
    /// <summary>
    /// Method calculates and returns winning team of current game
    /// </summary>
    /// <param name="game">
    /// GameModel representing the current game
    /// </param>
    /// <param name="favoriteScore">
    /// int representing the favorite team score of the current game
    /// </param>
    /// <param name="underdogScore">
    /// int representing the underdog team score of the current game
    /// </param>
    /// <returns name="winner">
    /// TeamModel representing the winner of the current game
    /// </returns>
    public static async Task<TeamModel> CalculateWinningTeam(
        GameModel game, double favoriteScore, double underdogScore, 
            ITeamData teamData)
    {
        TeamModel? winner = new();

        TeamModel? favorite = await teamData.GetTeam(game.FavoriteId);
        TeamModel? underdog = await teamData.GetTeam(game.UnderdogId);

        winner = (favoriteScore == underdogScore) ? null :
            (favoriteScore > underdogScore) ? favorite :
                underdog;

        return winner;
    }

    /// <summary>
    /// Method calculates and returns the winning team
    /// after factoring in the point spread
    /// </summary>
    /// <param name="game">
    /// GameModel representing the current game
    /// </param>
    /// <param name="favoriteScore">
    /// int representing the favorite team score
    /// </param>
    /// <param name="underdogScore">
    /// int representing the underdog team score
    /// </param>
    /// <returns name="winner">
    /// winner of the game after factoring in the point spread
    /// </returns>
    public static async Task<TeamModel> CalculateWinningTeamForBet(
        GameModel game, double favoriteScore, double underdogScore, 
            ITeamData teamData)
    {
        double pointSpread = game.PointSpread;
        double favoriteScoreMinusPointSpread = favoriteScore - pointSpread;

        TeamModel? favorite = await teamData.GetTeam(game.FavoriteId);
        TeamModel? underdog = await teamData.GetTeam(game.UnderdogId);

        TeamModel? winner = new();

        winner = (favoriteScoreMinusPointSpread == underdogScore) ? null :
            (favoriteScoreMinusPointSpread > underdogScore) ? favorite :
                underdog;

        return winner;
    }
}
