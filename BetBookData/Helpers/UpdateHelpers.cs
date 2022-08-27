
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Helpers;

#nullable enable

public static class UpdateHelpers
{
    public static async Task UpdateParleyBetWinners(this IMediator mediator)
    {
        IEnumerable<GameModel> games = await mediator.Send(new GetGamesQuery());
        IEnumerable<TeamModel> teams = await mediator.Send(new GetTeamsQuery());
        IEnumerable<BetModel> bets = await mediator.Send(new GetBetsQuery());
        IEnumerable<ParleyBetModel> parleyBets = await mediator.Send(
            new GetParleyBetsQuery());

        List<ParleyBetModel> parleyBetsInProgress = parleyBets.Where(pb =>
                pb.ParleyBetStatus == ParleyBetStatus.IN_PROGRESS).ToList();

        parleyBetsInProgress = 
            parleyBetsInProgress.PopulateParleyBetsWithBetsWithGamesAndTeams(games, teams, bets);

        foreach (ParleyBetModel parleyBet in parleyBetsInProgress)
        {
            if (parleyBet.CheckIfParleyBetLoser())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.LOSER;
                await mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }

            if (parleyBet.CheckIfParleyBetWinner())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.WINNER;
                await mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }

            if (parleyBet.CheckIfParleyBetPush())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.PUSH;
                await mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }
        }
    }

    public static async Task UpdateBettors(
        this GameModel currentGame, double? homeTeamFinalScore,
        double? awayTeamFinalScore, IEnumerable<GameModel> games, IMediator mediator)
    {
        IEnumerable<TeamModel> teams = await mediator.Send(new GetTeamsQuery());
        IEnumerable<BetModel> bets = await mediator.Send(new GetBetsQuery());

        List<BetModel> betsOnCurrentGame = bets.Where(b =>
            b.GameId == currentGame.Id).ToList();

        betsOnCurrentGame = 
            betsOnCurrentGame.PopulateBetModelsWithGamesAndTeams(games, teams);

        foreach(BetModel bet in betsOnCurrentGame)
        {
            TeamModel? winningTeamForBet = bet.CalculateWinnerForBet(
                    currentGame, homeTeamFinalScore, awayTeamFinalScore, teams);

            if(winningTeamForBet is null)
            {
                bet.BetStatus = BetStatus.PUSH;
                bet.FinalWinnerId = 0;

                await mediator.Send(new UpdateBetCommand(bet));
                continue;
            }

            bet.FinalWinnerId = winningTeamForBet.Id;

            bet.BetStatus = winningTeamForBet.Id == bet.ChosenWinnerId ? BetStatus.WINNER
                            : BetStatus.LOSER;

            await mediator.Send(new UpdateBetCommand(bet));
        }
    }

    public static async Task UpdateTeamRecords(
        this GameModel currentGame, double? homeTeamFinalScore, 
        double? awayTeamFinalScore, IMediator mediator)
    {
        IEnumerable<TeamModel> teams = await mediator.Send(new GetTeamsQuery());

        if (DateTime.Now > new DateTime(2022, 9, 7))
        {
            TeamModel? homeTeam = 
                    teams.Where(t => t.Id == currentGame.HomeTeamId).FirstOrDefault();
            TeamModel? awayTeam = 
                    teams.Where(t => t.Id == currentGame.AwayTeamId).FirstOrDefault();

            if (homeTeam is null || awayTeam is null)
                    return;

            TeamModel? actualWinningTeam = 
                currentGame.CalculateWinningTeam(homeTeamFinalScore, awayTeamFinalScore, teams);

            // If game is a draw
            if (actualWinningTeam is null)
            {
                homeTeam.Draws += $"{awayTeam.TeamName}|";
                awayTeam.Draws += $"{homeTeam.TeamName}|";

                await mediator.Send(new UpdateTeamCommand(homeTeam));
                await mediator.Send(new UpdateTeamCommand(awayTeam));
                return;
            }

            // If game is not a draw
            TeamModel actualLosingTeam = actualWinningTeam == homeTeam ? awayTeam : homeTeam;

            actualWinningTeam.Wins += $"{actualLosingTeam.TeamName}|";
            actualLosingTeam.Losses += $"{actualWinningTeam.TeamName}|";

            await mediator.Send(new UpdateTeamCommand(actualWinningTeam));
            await mediator.Send(new UpdateTeamCommand(actualLosingTeam));
        }
        
    }

    public static async Task UpdateScores(
        this GameModel currentGame, double? homeTeamScore,
        double? awayTeamScore, IMediator mediator)
    {
        IEnumerable<TeamModel> teams = await mediator.Send(new GetTeamsQuery());

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

        await mediator.Send(new UpdateGameCommand(currentGame));
    }
}


#nullable restore
