using BetBookData.Models;

namespace BetBookData.Helpers;


#nullable enable

public static class CalculationHelpers
{
    public static TeamModel CalculateWinningTeam(
        this GameModel game, double? homeTeamFinalScore,
        double? awayTeamFinalScore, IEnumerable<TeamModel> teams)
    {

        TeamModel? homeTeam = 
            teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault();
        TeamModel? awayTeam = 
            teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault();

        TeamModel? winner = (homeTeamFinalScore == awayTeamFinalScore) ? null :
            (homeTeamFinalScore > awayTeamFinalScore) ? homeTeam :
                awayTeam;

        return winner!;
    }

    public static TeamModel CalculateWinnerForBet(
        this BetModel bet, GameModel game, double? homeTeamFinalScore,
        double? awayTeamFinalScore, IEnumerable<TeamModel> teams)
    {
        double? homeTeamScoreAfterPointSpreaad = homeTeamFinalScore + bet.PointSpread;

        TeamModel? homeTeam = teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault();
        TeamModel? awayTeam = teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault();

        TeamModel? winner = homeTeamScoreAfterPointSpreaad == awayTeamFinalScore ? null :
            homeTeamScoreAfterPointSpreaad > awayTeamFinalScore ? homeTeam :
                awayTeam;

        return winner!;
    }

    public static int CalculateWeek(this SeasonType season, DateTime dateTime)
    {
        int week = season == SeasonType.PRE ? (dateTime - new DateTime(2022, 8, 9)).Days / 7 
                   : season == SeasonType.REG ? (dateTime - new DateTime(2022, 9, 1)).Days / 7 
                   : (dateTime - new DateTime(2023, 1, 14)).Days / 7;

        if (week < 0)
            return 0;

        return week + 1;
    }

    public static SeasonType CalculateSeason(this DateTime dateTime)
    {
        return dateTime > new DateTime(2022, 8, 9) && dateTime < new DateTime(2022, 9, 1) ? SeasonType.PRE 
            : dateTime > new DateTime(2022, 9, 8) && dateTime < new DateTime(2023, 1, 14) ? SeasonType.REG 
            : SeasonType.POST;
    }

    public static decimal CalculateTotalPendingRefund(
            this List<BetModel> pushBets)
    {
        if (pushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (BetModel bet in pushBets)
            total += bet.BetPayout;

        return total;
    }

    public static decimal CalculateTotalPendingPayout(
            this List<BetModel> winningBets)
    {
        if (winningBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (BetModel bet in winningBets)
            total += (bet.BetPayout + bet.BetAmount);

        return total;
    }

    public static decimal CalculateBetPayout(this decimal betAmount)
    {
        return (betAmount - betAmount * (decimal).1) + betAmount;
    }

    public static decimal CalculateParleyBetPayout(
            this int gameCount, decimal betAmount)
    {
        if (gameCount < 2)
            return 0;

        betAmount -= betAmount * (decimal).1;

        return gameCount == 2 ? (betAmount * (decimal)2.6) + betAmount 
            : gameCount == 3 ? (betAmount * (decimal)6) + betAmount 
            : gameCount == 4 ? (betAmount * (decimal)11) + betAmount 
            : (betAmount * (decimal)22) + betAmount;
    }

    public static decimal CalculateTotalPendingParleyRefund(
        this List<ParleyBetModel> parleyPushBets)
    {
        if (parleyPushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (ParleyBetModel bet in parleyPushBets)
            total += bet.BetPayout;

        return total;
    }

    public static decimal CalculateTotalPendingParleyPayout(
        this List<ParleyBetModel> parleyWinningBets)
    {
        if (parleyWinningBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (ParleyBetModel parleyBet in parleyWinningBets)
            total += (parleyBet.BetPayout + parleyBet.BetAmount);

        return total;
    }
}

#nullable restore
