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
                return;
            }

            else if (pb.CheckIfParleyBetWinner())
            {
                pb.ParleyBetStatus = ParleyBetStatus.WINNER;
                await parleyData.UpdateParleyBet(pb);
                return;
            }

            else if (pb.CheckIfParleyBetPush())
            {
                pb.ParleyBetStatus = ParleyBetStatus.PUSH;
                await parleyData.UpdateParleyBet(pb);
                return;
            }
        }
    }

    /// <summary>
    /// Async method updates final winner and bet status 
    /// for all bets that were placed on current game
    /// </summary>
    /// <param name="favoriteScore">
    /// double represents the favorite team score
    /// </param>
    /// <param name="underdogScore">
    /// double represents the underdog team score
    /// </param>
    /// <returns></returns>
    public static async Task UpdateBetWinners(
        this GameModel currentGame, double homeTeamFinalScore,
            double awayTeamFinalScore, IBetData betData, IEnumerable<GameModel> games, IEnumerable<TeamModel> teams, IEnumerable<BetModel> bets)
    {

        List<BetModel> betsOnCurrentGame = bets.Where(b =>
            b.GameId == currentGame.Id).ToList();

        betsOnCurrentGame = 
            betsOnCurrentGame.PopulateBetModelsWithGamesAndTeams(games, teams);

        TeamModel? winningTeamForBets = currentGame.CalculateWinningTeamForBet(
                homeTeamFinalScore, awayTeamFinalScore, teams);

        // Bets are winners or losers
        if (winningTeamForBets is not null)
        {
            foreach (BetModel bet in betsOnCurrentGame)
            {
                bet.FinalWinnerId = winningTeamForBets.Id;

                if (winningTeamForBets.Id == bet.ChosenWinnerId)
                    bet.BetStatus = BetStatus.WINNER;

                else
                    bet.BetStatus = BetStatus.LOSER;

                await betData.UpdateBet(bet);
            }
        }

        // Bets are a push
        else
        {
            foreach (BetModel bet in betsOnCurrentGame)
            {
                bet.BetStatus = BetStatus.PUSH;
                bet.FinalWinnerId = 0;

                await betData.UpdateBet(bet);
            }
        }
    }

    /// <summary>
    /// Async method updates team records of
    /// the actual winning  team and actual losing team
    /// </summary>
    /// <param name="favoriteScore">
    /// int representing the favorite team score of the current game
    /// </param>
    /// <param name="underdogScore">
    /// int representing the underdog team score of the current game
    /// </param>
    /// <returns></returns>
    public static async Task UpdateTeamRecords(this GameModel currentGame,
        double homeTeamFinalScore, double awayTeamFinalScore, 
            IEnumerable<TeamModel> teams, ITeamData teamData)
    {
        TeamModel? homeTeam = teams.Where(t => t.Id == currentGame.HomeTeamId).FirstOrDefault();
        TeamModel? awayTeam = teams.Where(t => t.Id == currentGame.AwayTeamId).FirstOrDefault();

        if (homeTeam is not null && awayTeam is not null)
        {
            TeamModel? actualWinningTeam = new();
            TeamModel? actualLosingTeam = new();

            if (homeTeamFinalScore > awayTeamFinalScore)
            {
                actualWinningTeam = homeTeam;
                actualLosingTeam = awayTeam;
            }

            else if (homeTeamFinalScore < awayTeamFinalScore)
            {
                actualWinningTeam = awayTeam;
                actualLosingTeam = homeTeam;
            }

            else if (homeTeamFinalScore == awayTeamFinalScore)
                actualWinningTeam = null;

            // If game is a draw
            if (actualWinningTeam is null)
            {
                homeTeam.Draws += $"{awayTeam.TeamName}|";
                awayTeam.Draws += $"{homeTeam.TeamName}|";

                await teamData.UpdateTeam(homeTeam);
                await teamData.UpdateTeam(awayTeam);
            }

            // If game is not a draw
            else
            {
                actualWinningTeam.Wins += $"{actualLosingTeam.TeamName}|";
                actualLosingTeam.Losses += $"{actualWinningTeam.TeamName}|";

                await teamData.UpdateTeam(actualWinningTeam);
                await teamData.UpdateTeam(actualLosingTeam);
            }
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
        if (currentGame.GameStatus != GameStatus.FINISHED)
        {
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
}
