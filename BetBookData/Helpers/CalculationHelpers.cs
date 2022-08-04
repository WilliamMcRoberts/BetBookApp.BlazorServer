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
    /// <param name="teams"></param>
    /// <returns>TeamModel</returns>
    public static TeamModel CalculateWinningTeam(
        this GameModel game, double homeTeamFinalScore, double awayTeamFinalScore, 
            IEnumerable<TeamModel> teams)
    {

        TeamModel? homeTeam = teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault();
        TeamModel? awayTeam = teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault();

        TeamModel? winner = (homeTeamFinalScore == awayTeamFinalScore) ? null :
            (homeTeamFinalScore > awayTeamFinalScore) ? homeTeam :
                awayTeam;

        return winner!;
    }

    /// <summary>
    /// Method calculates and returns the winning team
    /// after factoring in the point spread
    /// </summary>
    /// <param name="game"></param>
    /// <param name="favoriteScore"></param>
    /// <param name="underdogScore"></param>
    /// <param name="teams"></param>
    /// <returns>TeamModel</returns>
    public static TeamModel CalculateWinningTeamForBet(
        this GameModel game, double homeTeamFinalScore, double awayTeamFinalScore, 
            IEnumerable<TeamModel> teams)
    {
        double pointSpread = game.PointSpread;
        double homeTeamScoreAfterPointSpreaad = homeTeamFinalScore + pointSpread;

        TeamModel? homeTeam = teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault();
        TeamModel? awayTeam = teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault();

        TeamModel? winner = (homeTeamScoreAfterPointSpreaad == awayTeamFinalScore) ? null :
            (homeTeamScoreAfterPointSpreaad > awayTeamFinalScore) ? homeTeam :
                awayTeam;

        return winner!;
    }

    /// <summary>
    /// Method calculates and returns week of season given a certain date
    /// </summary>
    /// <param name = "season" ></ param >
    /// < param name="dateTime"></param>
    /// <returns></returns>
    public static int CalculateWeek(this SeasonType season, DateTime dateTime)
    {
        DateTime preSeasonStartDate = new DateTime(2022, 8, 4);
        DateTime regularSeasonStartDate = new DateTime(2022, 9, 8);
        DateTime postSeasonStartDate = new DateTime(2023, 1, 14);

        int week = 0;
        
        if (season == SeasonType.PRE)
            week = (dateTime - preSeasonStartDate).Days / 7;
            
        else if (season == SeasonType.REG)
            week = (dateTime - regularSeasonStartDate).Days / 7;

        else if (season == SeasonType.POST)
            week = (dateTime - postSeasonStartDate).Days / 7;

        if (week < 0)
            return 0;

        return week;
    }

    public static (int,SeasonType) CalculateNextWeekAndSeasonFromCurrentWeekAndSeason(
            this (int currentWeek, SeasonType currentSeason) weekSeason)
    {
        (int, SeasonType) newWeekSeason;

        if (weekSeason == (4, SeasonType.PRE))
            newWeekSeason = (1, SeasonType.REG);

        else if (weekSeason == (17, SeasonType.REG))
            newWeekSeason = (1, SeasonType.POST);

        else
            newWeekSeason = 
                (weekSeason.currentWeek + 1, weekSeason.currentSeason);

        return newWeekSeason;
    }

    /// <summary>
    /// Method calculates the season of provided DateTime 
    /// </summary>
    /// <param name = "dateTime" > DateTime represents date to calculate</param>
    /// <returns>SeasonType represents the type of season</returns>
    public static SeasonType CalculateSeason(this DateTime dateTime)
    {
        DateTime preSeasonStartDate = new DateTime(2022, 8, 4);
        DateTime regularSeasonStartDate = new DateTime(2022, 9, 8);
        DateTime postSeasonStartDate = new DateTime(2023, 1, 14);

        SeasonType result = new();

        if (dateTime > preSeasonStartDate && dateTime < regularSeasonStartDate)
            result = SeasonType.PRE;

        else if (dateTime > regularSeasonStartDate && dateTime < postSeasonStartDate)
            result = SeasonType.REG;

        else if (dateTime > postSeasonStartDate)
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

        return total = Convert.ToDecimal((total).ToString("#.00"));
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

        return totalPayout = Convert.ToDecimal((total).ToString("#.00"));
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

        return Convert.ToDecimal((payout).ToString("#,##0.00"));
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

        return Convert.ToDecimal((total).ToString("#,##0.00"));
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

        foreach (ParleyBetModel parleyBet in parleyWinningBets)
            total += (parleyBet.BetPayout + parleyBet.BetAmount);

        return Convert.ToDecimal((total).ToString("#,##0.00"));
    }
}
