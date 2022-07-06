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
        this GameModel game, double favoriteScore, double underdogScore, 
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
        this GameModel game, double favoriteScore, double underdogScore, 
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

    /// <summary>
    /// Method calculates and returns current week in current season 
    /// (PRE, REG, POST)
    /// </summary>
    /// <returns>
    /// int represents the current week
    /// </returns>
    public static int CalculateWeek(this SeasonType season, DateTime dateTime)
    {
        DateTime preSeasonStartDate = new DateTime(2022, 8, 4);
        DateTime regularSeasonStartDate = new DateTime(2022, 9, 8);
        DateTime postSeasonStartDate = new DateTime(2023, 1, 14);

        int week = 0;

        if (season == SeasonType.PRE)
        {
            TimeSpan span = dateTime - preSeasonStartDate;
            week = span.Days / 7;
        }

        else if (season == SeasonType.REG)
        {
            TimeSpan span = dateTime - regularSeasonStartDate;
            week = span.Days / 7;
        }

        else if (season == SeasonType.POST)
        {
            TimeSpan span = dateTime - postSeasonStartDate;
            week = span.Days / 7;
        }

        if(week < 0)
            return 0;

        return week;
    }

    /// <summary>
    /// Method calculates the season of provided DateTime 
    /// </summary>
    /// <param name="dateTime">DateTime represents date to calculate</param>
    /// <returns>SeasonType represents the type of season</returns>
    public static SeasonType CalculateSeason(this DateTime dateTime)
    {
        DateTime preSeasonStartDate = new DateTime(2022, 8, 4);
        DateTime regularSeasonStartDate = new DateTime(2022, 9, 8);
        DateTime postSeasonStartDate = new DateTime(2023, 1, 14);
        DateTime superBowlDay = new DateTime(2023, 2, 5);

        SeasonType result = new();

        if (dateTime > preSeasonStartDate && dateTime < regularSeasonStartDate)
            result = SeasonType.PRE;
        else if (dateTime > regularSeasonStartDate && dateTime < postSeasonStartDate)
            result = SeasonType.REG;
        else if (dateTime > postSeasonStartDate && dateTime < superBowlDay)
            result = SeasonType.POST;

        return result;
    }
}
