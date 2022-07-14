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
    public static async Task UpdateParleyBetWinners(IParleyBetData parleyData)
    {
        IEnumerable<ParleyBetModel> allParleyBets = await parleyData.GetParleyBets();

        List<ParleyBetModel> parleyBetsInProgress = allParleyBets.Where(pb =>
            pb.ParleyBetStatus == ParleyBetStatus.IN_PROGRESS).ToList();

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
        this GameModel currentGame, double favoriteScore,
            double underdogScore, IBetData betData, ITeamData teamData)
    {

        IEnumerable<BetModel> bets = await betData.GetBets();

        List<BetModel> betsOnCurrentGame = bets.Where(b =>
            b.GameId == currentGame.Id).ToList();

        TeamModel? winningTeamForBets =
            await currentGame.CalculateWinningTeamForBet(
                favoriteScore, underdogScore, teamData);

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
        double favoriteScore, double underdogScore, ITeamData teamData, 
            ITeamRecordData recordData)
    {
        TeamModel? currentFavoriteTeam =
            await teamData.GetTeam(currentGame.FavoriteId);
        TeamModel? currentUnderdogTeam =
            await teamData.GetTeam(currentGame.UnderdogId);

        if (currentFavoriteTeam is not null && currentUnderdogTeam is not null)
        {
            TeamModel? actualWinningTeam = new();
            TeamModel? actualLosingTeam = new();

            if (favoriteScore > underdogScore)
            {
                actualWinningTeam = currentFavoriteTeam;
                actualLosingTeam = currentUnderdogTeam;
            }

            else if (favoriteScore < underdogScore)
            {
                actualWinningTeam = currentUnderdogTeam;
                actualLosingTeam = currentFavoriteTeam;
            }

            else if (favoriteScore == underdogScore)
                actualWinningTeam = null;

            // If game is a draw
            if (actualWinningTeam is null)
            {
                TeamRecordModel? teamFavoriteRecord =
                    await recordData.GetTeamRecord(currentFavoriteTeam.Id);
                TeamRecordModel? teamUnderdogRecord =
                    await recordData.GetTeamRecord(currentUnderdogTeam.Id);

                if (teamFavoriteRecord is not null && teamUnderdogRecord is not null)
                {
                    teamFavoriteRecord.Draws += $"{currentUnderdogTeam.TeamName}|";
                    teamUnderdogRecord.Draws += $"{currentFavoriteTeam.TeamName}|";

                    await recordData.UpdateTeamRecord(teamFavoriteRecord);
                    await recordData.UpdateTeamRecord(teamUnderdogRecord);
                }
            }

            // If game is not a draw
            else
            {
                TeamRecordModel? winnerTeamRecord =
                    await recordData.GetTeamRecord(actualWinningTeam.Id);
                TeamRecordModel? loserTeamRecord =
                    await recordData.GetTeamRecord(actualLosingTeam.Id);

                if (winnerTeamRecord is not null && loserTeamRecord is not null)
                {
                    winnerTeamRecord.Wins += $"{actualLosingTeam.TeamName}|";
                    loserTeamRecord.Losses += $"{actualWinningTeam.TeamName}|";

                    await recordData.UpdateTeamRecord(winnerTeamRecord);
                    await recordData.UpdateTeamRecord(loserTeamRecord);
                }
            }
        }
    }

    /// <summary>
    /// Async method updates game with scores and statuses
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> UpdateScores(this GameModel currentGame, 
        int favoriteTeamScore, int underdogTeamScore, ITeamData teamData, 
            IGameData gameData)
    {
        if (currentGame.GameStatus != GameStatus.FINISHED)
        {
            currentGame.FavoriteFinalScore = favoriteTeamScore;
            currentGame.UnderdogFinalScore = underdogTeamScore;
            currentGame.GameStatus = GameStatus.FINISHED;

            TeamModel? gameWinner =
                await currentGame.CalculateWinningTeam(favoriteTeamScore,
                        underdogTeamScore, teamData);

            if (gameWinner is not null)
                currentGame.GameWinnerId = gameWinner.Id;

            await gameData.UpdateGame(currentGame);

            return false;
        }

        else
            return true;
    }
}
