using BetBookData.Models;

namespace BetBookData.Helpers;


#nullable enable

public static class CalculationHelpers
{
    #nullable disable
    public static TeamModel CalculateWinnerOfGame(this GameModel _game)
    {
        return 
            _game.HomeTeamFinalScore == _game.AwayTeamFinalScore ? null :
            _game.HomeTeamFinalScore > _game.AwayTeamFinalScore ? _game.HomeTeam 
            : _game.AwayTeam;
    }

    public static TeamModel CalculateWinnerForBet(this BetModel _bet)
    {
        double? homeTeamScoreAfterPointSpread =
                _bet.Game?.HomeTeamFinalScore + _bet.Game?.PointSpread;

        return  homeTeamScoreAfterPointSpread == _bet.Game?.AwayTeamFinalScore ? null :
                homeTeamScoreAfterPointSpread > _bet.Game?.AwayTeamFinalScore ? _bet.Game?.HomeTeam :
                _bet.Game?.AwayTeam;
    }
    #nullable enable

    public static int CalculateWeek(this Season season, DateTime dateTime)
    {
        int week = season == Season.PRE ? (dateTime - new DateTime(2022, 8, 9)).Days / 7 
                   : season == Season.REG ? (dateTime - new DateTime(2022, 9, 6)).Days / 7 
                   : (dateTime - new DateTime(2023, 1, 14)).Days / 7;

        if (week < 0)
            return 0;

        return week + 1;
    }

    public static Season CalculateSeason(this DateTime dateTime)
    {
        return dateTime > new DateTime(2022, 8, 9) && dateTime < new DateTime(2022, 8, 28) ? Season.PRE 
            : dateTime > new DateTime(2022, 8, 31) && dateTime < new DateTime(2023, 1, 14) ? Season.REG 
            : Season.POST;
    }

    public static decimal CalculateTotalPendingRefund(
            this List<BetModel> _pushBets)
    {
        if (_pushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (BetModel bet in _pushBets)
            total += bet.BetAmount;

        return total -= total * (decimal).1;
    }

    public static decimal CalculateTotalPendingPayout(
            this List<BetModel> _winningBets)
    {
        if (_winningBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (BetModel bet in _winningBets)
            total += bet.BetPayout;

        return total;
    }

    public static decimal CalculateBetPayout(this decimal _betAmount)
    {
        return (_betAmount - _betAmount * (decimal).1) + _betAmount;
    }

    public static decimal CalculateParleyBetPayout(
            this int _gameCount, decimal _betAmount)
    {
        if (_gameCount < 2)
            return 0;

        _betAmount -= _betAmount * (decimal).1;

        return _gameCount == 2 ? (_betAmount * (decimal)2.6) + _betAmount
            : _gameCount == 3 ? (_betAmount * (decimal)6) + _betAmount
            : _gameCount == 4 ? (_betAmount * (decimal)11) + _betAmount
            : (_betAmount * (decimal)22) + _betAmount;
    }

    public static decimal CalculateTotalPendingParleyRefund(
        this List<ParleyBetModel> _parleyPushBets)
    {
        if (_parleyPushBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (ParleyBetModel bet in _parleyPushBets)
            total += bet.BetAmount;

        return total -= total * (decimal).1;
    }

    public static decimal CalculateTotalPendingParleyPayout(
        this List<ParleyBetModel> _parleyWinningBets)
    {
        if (_parleyWinningBets.Count == 0)
            return 0;

        decimal total = 0;

        foreach (ParleyBetModel parleyBet in _parleyWinningBets)
            total += parleyBet.BetPayout;

        return total;
    }
}

#nullable restore
