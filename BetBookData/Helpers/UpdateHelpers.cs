
using BetBookData.Commands.UpdateCommands;
using BetBookData.Commands.InsertCommands;
using BetBookData.Dto;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Helpers;

#nullable enable

public static class UpdateHelpers
{
    public static async Task UpdateParleyBetWinners(this IMediator _mediator)
    {
        List<ParleyBetModel> parleyBetsInProgress =
            (List<ParleyBetModel>)await _mediator.Send(new GetInProgressParleyBetsQuery());

        foreach (ParleyBetModel parleyBet in parleyBetsInProgress)
        {
            if (parleyBet.CheckIfParleyBetLoser())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.LOSER;
                await _mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }

            if (parleyBet.CheckIfParleyBetWinner())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.WINNER;
                await _mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }

            if (parleyBet.CheckIfParleyBetPush())
            {
                parleyBet.ParleyBetStatus = ParleyBetStatus.PUSH;
                await _mediator.Send(new UpdateParleyBetCommand(parleyBet));
                continue;
            }
        }
    }

    public static async Task UpdateBettors(
        this IMediator _mediator, GameModel _currentGame)
    {
        List<BetModel> betsOnCurrentGame = 
            (List<BetModel>)await _mediator.Send(
                new GetBetsOnCurrentGameQuery(_currentGame.Id));

        foreach(BetModel bet in betsOnCurrentGame)
        {
            TeamModel? winningTeamForBet = 
                bet.CalculateWinnerForBet();

            if(winningTeamForBet is null)
            {
                bet.BetStatus = BetStatus.PUSH;
                bet.FinalWinnerId = 0;

                await _mediator.Send(new UpdateBetCommand(bet));
                continue;
            }

            bet.FinalWinnerId = winningTeamForBet.Id;

            bet.BetStatus = winningTeamForBet.Id == bet.ChosenWinnerId ? BetStatus.WINNER
                            : BetStatus.LOSER;

            await _mediator.Send(new UpdateBetCommand(bet));
        }
    }

    public static async Task UpdateTeamRecords(
        this IMediator _mediator, GameModel _currentGame)
    {
        if (DateTime.Now <= new DateTime(2022, 9, 7)) return;

        TeamModel? actualWinningTeam = 
            _currentGame.CalculateWinnerOfGame();

        // If game is a draw
        if (actualWinningTeam is null)
        {
            _currentGame.HomeTeam!.Draws += $"{_currentGame.AwayTeam!.TeamName}|";
            _currentGame.AwayTeam.Draws += $"{_currentGame.HomeTeam.TeamName}|";
            _currentGame.GameStatus = GameStatus.FINISHED;


            await _mediator.Send(new UpdateTeamCommand(_currentGame.HomeTeam));
            await _mediator.Send(new UpdateTeamCommand(_currentGame.AwayTeam));
            await _mediator.Send(new UpdateGameCommand(_currentGame));

            return;
        }

        // If game is not a draw
        TeamModel? actualLosingTeam = 
            actualWinningTeam == _currentGame.HomeTeam ? _currentGame.AwayTeam 
            : _currentGame.HomeTeam;

        actualWinningTeam.Wins += $"{actualLosingTeam?.TeamName}|";
        actualLosingTeam!.Losses += $"{actualWinningTeam.TeamName}|";

        _currentGame.GameStatus = GameStatus.FINISHED;
        _currentGame.GameWinner = actualWinningTeam;
        _currentGame.GameWinnerId = actualWinningTeam.Id;


        await _mediator.Send(new UpdateTeamCommand(actualWinningTeam));
        await _mediator.Send(new UpdateTeamCommand(actualLosingTeam));
        await _mediator.Send(new UpdateGameCommand(_currentGame));
    }

    public static async Task UpdateFinishedGameScoresTeamRecordsSingleBettorsParleyBettors(
        this IMediator _mediator, List<GameDto> _finishedGameDtoList, 
            List<GameModel> _thisWeeksGamesNotFinished)
    {
        foreach(GameDto gameDto in _finishedGameDtoList)
        {
            if (gameDto.HomeScore is null || gameDto.AwayScore is null)
                continue;

            GameModel gameModel = _thisWeeksGamesNotFinished.Where(g =>
                    g.ScoreId == gameDto.ScoreID).FirstOrDefault()!;

            if (gameModel is null) continue;

            if (!double.TryParse(gameDto.HomeScore?.ToString(), out var homeScore))
                continue;
            if (!double.TryParse(gameDto.AwayScore?.ToString(), out var awayScore))
                continue;

            gameModel.HomeTeamFinalScore = homeScore;
            gameModel.AwayTeamFinalScore = awayScore;
            
            await _mediator.UpdateTeamRecords(gameModel);
            await _mediator.UpdateBettors(gameModel);
        }

        await _mediator.UpdateParleyBetWinners();
    }

    public static async Task UpdateThisWeeksAvailableGameList(
        this IMediator _mediator, GameDto[] _gameDtoArray, 
            List<GameModel> _thisWeeksUnfinishedGameModelList)
    {
        List<TeamModel> teams =
            (List<TeamModel>)await _mediator.Send(new GetTeamsQuery());

        Season season = DateTime.Now.CalculateSeason();

        foreach (GameDto game in _gameDtoArray.Where(g => g.IsOver == false))
        {
            if (game.PointSpread is null) continue;

            GameModel gameModel = _thisWeeksUnfinishedGameModelList.Where(g =>
                    g.ScoreId == game.ScoreID).FirstOrDefault()!;

            if (gameModel is not null &&
                Math.Round(Convert.ToDouble(gameModel.PointSpread)) != 
                    Math.Round(Convert.ToDouble(game.PointSpread)))
            {
                gameModel.PointSpread = Math.Round(Convert.ToDouble(game.PointSpread), 1);
                await _mediator.Send(new UpdateGameCommand(gameModel));
            }

            if (gameModel is not null)
                continue;

            GameModel newGame = new()
            {
                AwayTeamId = teams.Where(t => t.Symbol == game.AwayTeam).FirstOrDefault()!.Id,
                HomeTeamId = teams.Where(t => t.Symbol == game.HomeTeam).FirstOrDefault()!.Id,
                Stadium = game.StadiumDetails.Name,
                PointSpread = Math.Round(Convert.ToDouble(game.PointSpread), 1),
                WeekNumber = game.Week,
                Season = season,
                DateOfGame = game.DateTime,
                GameStatus = GameStatus.NOT_STARTED,
                ScoreId = game.ScoreID,
                DateOfGameOnly = game.DateTime.ToString("MM-dd"),
                TimeOfGameOnly = game.DateTime.ToString("hh:mm")
            };

            newGame = await _mediator.Send(new InsertGameCommand(newGame));
        }
    }

    public static async Task UpdateAll(
        this IMediator _mediator, int _week, Season _season)
    {
        GameDto[] gameDtoArray = 
            await _mediator.Send(new GetGameDtoArrayByWeekAndSeasonQuery(_week, _season));

        List<GameDto> finishedGameDtos =
            gameDtoArray.Where(g => g.IsOver == true).ToList();

        List<GameModel> thisWeeksGames =
            (List<GameModel>)await _mediator.Send(
                new GetGameModelsByWeekAndSeasonQuery(_week, _season));

        thisWeeksGames =
            thisWeeksGames.Where(g => g.GameStatus != GameStatus.FINISHED).ToList();

        await _mediator.UpdateThisWeeksAvailableGameList(
            gameDtoArray, thisWeeksGames);

        if (finishedGameDtos.Count > 0)
            await _mediator.UpdateFinishedGameScoresTeamRecordsSingleBettorsParleyBettors(
                finishedGameDtos, thisWeeksGames);
    }
}


#nullable restore
