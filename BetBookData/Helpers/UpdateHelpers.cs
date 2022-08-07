using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetBookData.Interfaces;
using BetBookData.Models;


namespace BetBookData.Helpers;
public static class UpdateHelpers
{
    public static async Task UpdateParleyBetWinners(
        this IParleyBetData parleyData, IEnumerable<ParleyBetModel> parleyBets, 
            IEnumerable<GameModel> games, IEnumerable<TeamModel> teams, 
                IEnumerable<BetModel> bets)
    {

        List<ParleyBetModel> parleyBetsInProgress = parleyBets.Where(pb =>
            pb.ParleyBetStatus == ParleyBetStatus.IN_PROGRESS).ToList();

        parleyBetsInProgress = 
            parleyBetsInProgress.PopulateParleyBetsWithBetsWithGamesAndTeams(
                games, teams, bets);

        foreach (ParleyBetModel pb in parleyBetsInProgress)
        {
            if (pb.CheckIfParleyBetLoser())
            {
                pb.ParleyBetStatus = ParleyBetStatus.LOSER;
                await parleyData.UpdateParleyBet(pb);
                continue;
            }

            if (pb.CheckIfParleyBetWinner())
            {
                pb.ParleyBetStatus = ParleyBetStatus.WINNER;
                await parleyData.UpdateParleyBet(pb);
                continue;
            }

            if (pb.CheckIfParleyBetPush())
            {
                pb.ParleyBetStatus = ParleyBetStatus.PUSH;
                await parleyData.UpdateParleyBet(pb);
                continue;
            }
        }
    }


    public static async Task UpdateBetWinners(
        this GameModel currentGame, double homeTeamFinalScore,
            double awayTeamFinalScore, IBetData betData, IEnumerable<GameModel> games, 
                IEnumerable<TeamModel> teams, IEnumerable<BetModel> bets)
    {

        List<BetModel> betsOnCurrentGame = bets.Where(b =>
            b.GameId == currentGame.Id).ToList();

        betsOnCurrentGame = 
            betsOnCurrentGame.PopulateBetModelsWithGamesAndTeams(games, teams);

        TeamModel? winningTeamForBets = currentGame.CalculateWinningTeamForBet(
                homeTeamFinalScore, awayTeamFinalScore, teams);

        // Bets are a push
        if(winningTeamForBets is null)
        {
            foreach (BetModel bet in betsOnCurrentGame)
            {
                bet.BetStatus = BetStatus.PUSH;
                bet.FinalWinnerId = 0;

                await betData.UpdateBet(bet);
            }

            return;
        }

        // Bets are winners or losers
        foreach (BetModel bet in betsOnCurrentGame)
        {
            bet.FinalWinnerId = winningTeamForBets.Id;

            bet.BetStatus = winningTeamForBets.Id == bet.ChosenWinnerId ? BetStatus.WINNER 
                : BetStatus.LOSER;

            await betData.UpdateBet(bet);
        }
    }


    public static async Task UpdateTeamRecords(this GameModel currentGame,
        double homeTeamFinalScore, double awayTeamFinalScore, 
            IEnumerable<TeamModel> teams, ITeamData teamData)
    {
        if(DateTime.Now > new DateTime(2022, 9, 7))
        {
            TeamModel? homeTeam = teams.Where(t => t.Id == currentGame.HomeTeamId).FirstOrDefault();
            TeamModel? awayTeam = teams.Where(t => t.Id == currentGame.AwayTeamId).FirstOrDefault();

            if (homeTeam is null || awayTeam is null)
                return;

            TeamModel actualWinningTeam = 
                currentGame.CalculateWinningTeam(homeTeamFinalScore, awayTeamFinalScore, teams);
            TeamModel actualLosingTeam = 
                actualWinningTeam == homeTeam ? awayTeam : homeTeam;

            // If game is a draw
            if (actualWinningTeam is null)
            {
                homeTeam.Draws += $"{awayTeam.TeamName}|";
                awayTeam.Draws += $"{homeTeam.TeamName}|";

                await teamData.UpdateTeam(homeTeam);
                await teamData.UpdateTeam(awayTeam);
                return;
            }

            // If game is not a draw
            actualWinningTeam.Wins += $"{actualLosingTeam.TeamName}|";
            actualLosingTeam.Losses += $"{actualWinningTeam.TeamName}|";

            await teamData.UpdateTeam(actualWinningTeam);
            await teamData.UpdateTeam(actualLosingTeam);
        }
        
    }

    /// <summary>
    /// Async method updates game with scores and statuses
    /// </summary>
    /// <returns></returns>
    public static async Task UpdateScores(this GameModel currentGame, 
        double homeTeamScore, double awayTeamScore, IEnumerable<TeamModel> teams, 
            IGameData gameData)
    {
        if (currentGame.GameStatus == GameStatus.FINISHED)
            return;

        currentGame.HomeTeamFinalScore = homeTeamScore;
        currentGame.AwayTeamFinalScore = awayTeamScore;
        currentGame.GameStatus = GameStatus.FINISHED;

        TeamModel? gameWinner =
            currentGame.CalculateWinningTeam(homeTeamScore,
                    awayTeamScore, teams);

        if (gameWinner is not null)
            currentGame.GameWinnerId = gameWinner.Id;

        await gameData.UpdateGame(currentGame);
    }
}
