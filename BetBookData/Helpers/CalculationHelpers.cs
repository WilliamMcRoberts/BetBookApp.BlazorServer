using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookData.Helpers;

public static class CalculationHelpers
{
    /// <summary>
    /// Method calculates and returns winning team of current game
    /// </summary>
    /// <param name="game"></param>
    /// <param name="favoriteScore"></param>
    /// <param name="underdogScore"></param>
    /// <param name="teamData"></param>
    /// <returns>TeamModel</returns>
    public static async Task<TeamModel> CalculateWinningTeam(
        this GameModel game, double favoriteScore, double underdogScore, 
            ITeamData teamData)
    {
        TeamModel? winner;

        TeamModel? favorite = await teamData.GetTeam(game.FavoriteId);
        TeamModel? underdog = await teamData.GetTeam(game.UnderdogId);

        winner = (favoriteScore == underdogScore) ? null :
            (favoriteScore > underdogScore) ? favorite :
                underdog;

        return winner!;
    }

    /// <summary>
    /// Method calculates and returns the winning team
    /// after factoring in the point spread
    /// </summary>
    /// <param name="game"></param>
    /// <param name="favoriteScore"></param>
    /// <param name="underdogScore"></param>
    /// <param name="teamData"></param>
    /// <returns>TeamModel</returns>
    public static async Task<TeamModel> CalculateWinningTeamForBet(
        this GameModel game, double favoriteScore, double underdogScore, 
            ITeamData teamData)
    {
        double pointSpread = game.PointSpread;
        double favoriteScoreMinusPointSpread = favoriteScore - pointSpread;

        TeamModel? favorite = await teamData.GetTeam(game.FavoriteId);
        TeamModel? underdog = await teamData.GetTeam(game.UnderdogId);

        TeamModel? winner;

        winner = (favoriteScoreMinusPointSpread == underdogScore) ? null :
            (favoriteScoreMinusPointSpread > underdogScore) ? favorite :
                underdog;

        return winner!;
    }

    /// <summary>
    /// Method calculates and returns week of season given a certain date
    /// </summary>
    /// <param name="season"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
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

        return week + 1;
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

    /// <summary>
    /// Method calculates and returns the total pending 
    /// refund for all push bets made by current user
    /// </summary>
    /// <param name="pushBets"></param>
    /// <returns>decimal</returns>
    public static decimal CalculateTotalPendingRefund(this List<BetModel> pushBets)
    {
        if (pushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (BetModel bet in pushBets)
            total += bet.BetPayout;

        total = Convert.ToDecimal((total).ToString("#.00"));

        return total;
    }

    /// <summary>
    /// Method calculates and returns the total pending
    /// payout for all winning bets made by current user
    /// </summary>
    /// <param name="winningBets"></param>
    /// <returns>decimal</returns>
    public static decimal CalculateTotalPendingPayout(this List<BetModel> winningBets)
    {
        if (winningBets.Count == 0)
            return 0;

        decimal total = 0;
        decimal totalPayout;

        foreach (BetModel bet in winningBets)
            total += (bet.BetPayout + bet.BetAmount);

        totalPayout = Convert.ToDecimal((total).ToString("#.00"));

        return totalPayout;
    }

    /// <summary>
    /// Static method to calculate the payout of a parley bet
    /// </summary>
    /// <param name="gamecount"></param>
    /// <param name="betAmount"></param>
    /// <returns></returns>
    public static decimal CalculateParleyBetPayout(this int gamecount, decimal betAmount)
    {
        betAmount -= betAmount * (decimal).1;
        decimal payout = 0;

        if (gamecount == 2) payout = betAmount * (decimal)2.6;
        else if (gamecount == 3) payout = betAmount * (decimal)6;
        else if (gamecount == 4) payout = betAmount * (decimal)11;
        else if (gamecount == 5) payout = betAmount * (decimal)22;

        return payout;
    }

    /// <summary>
    /// Method calculates and returns the total pending 
    /// refund for all push bets made by current user
    /// </summary>
    /// <param name="pushBets"></param>
    /// <returns>decimal</returns>
    public static decimal CalculateTotalPendingParleyRefund(this List<ParleyBetModel> parleyPushBets)
    {
        if (parleyPushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (ParleyBetModel bet in parleyPushBets)
            total += bet.BetPayout;

        total = Convert.ToDecimal((total).ToString("#.00"));

        return total;
    }

    /// <summary>
    /// Method calculates and returns the total pending
    /// payout for all winning bets made by current user
    /// </summary>
    /// <param name="winningBets"></param>
    /// <returns>decimal</returns>
    public static decimal CalculateTotalPendingParleyPayout(this List<ParleyBetModel> parleyWinningBets)
    {
        if (parleyWinningBets.Count == 0)
            return 0;

        decimal total = 0;
        decimal totalPayout;

        foreach (ParleyBetModel parleyBet in parleyWinningBets)
            total += (parleyBet.BetPayout + parleyBet.BetAmount);

        totalPayout = Convert.ToDecimal((total).ToString("#.00"));

        return totalPayout;
    }
}
