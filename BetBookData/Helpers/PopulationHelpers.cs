using BetBookData.Models;

namespace BetBookData.Helpers;

#nullable enable

public static class PopulationHelpers
{
    public static HashSet<GameModel> PopulateGameModelsWithTeams(
        this HashSet<GameModel> gamesToPopulate, IEnumerable<TeamModel> teams)
    {

        foreach (GameModel game in gamesToPopulate)
        {
            game.HomeTeam = teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault()!;
            game.AwayTeam = teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault()!;

            if (game.GameWinnerId != 0)
                game.GameWinner = teams.Where(t => t.Id == game.GameWinnerId).FirstOrDefault()!;
        }

        return gamesToPopulate;
    }

    public static GameModel PopulateGameModelWithTeams(
        this GameModel gameToPopulate, IEnumerable<TeamModel> teams)
    {
        gameToPopulate.HomeTeam = teams.Where(t => t.Id == gameToPopulate.HomeTeamId).FirstOrDefault()!;
        gameToPopulate.AwayTeam = teams.Where(t => t.Id == gameToPopulate.AwayTeamId).FirstOrDefault()!;

        if (gameToPopulate.GameWinnerId != 0)
            gameToPopulate.GameWinner = teams.Where(t => t.Id == gameToPopulate.GameWinnerId).FirstOrDefault()!;

        return gameToPopulate;
    }

    public static List<BetModel> PopulateBetModelsWithGamesAndTeams(
        this List<BetModel> betsToPopulate, IEnumerable<GameModel> games, 
        IEnumerable<TeamModel> teams)
    {
        foreach (BetModel bet in betsToPopulate)
        {

            bet.Game = games.Where(g =>
                g.Id == bet?.GameId).FirstOrDefault()!.PopulateGameModelWithTeams(teams);
            bet.ChosenWinner = teams.Where(t =>
                t.Id == bet.ChosenWinnerId).FirstOrDefault();

            bet.FinalWinner =
                bet.FinalWinnerId != 0 ? teams.Where(t => t.Id == bet.FinalWinnerId).FirstOrDefault() : null;
        }

        return betsToPopulate;
    }

    public static List<ParleyBetModel> PopulateParleyBetsWithBetsWithGamesAndTeams(
        this List<ParleyBetModel> parleyBetsToPopulate, IEnumerable<GameModel> games,
        IEnumerable<TeamModel> teams, IEnumerable<BetModel> bets)
    {
        foreach (ParleyBetModel parleyBet in parleyBetsToPopulate)
        {
            BetModel bet1 = bets.Where(b => b.Id == parleyBet.Bet1Id).FirstOrDefault()!;
            BetModel bet2 = bets.Where(b => b.Id == parleyBet.Bet2Id).FirstOrDefault()!;

            if (bet1 is not null && bet2 is not null)
            {
                parleyBet.Bets.Add(bet1);
                parleyBet.Bets.Add(bet2);

                if (parleyBet.Bet3Id != 0)
                {
                    BetModel? bet3 = bets.Where(b => b.Id == parleyBet.Bet3Id).FirstOrDefault();

                    if (bet3 is not null)
                        parleyBet.Bets.Add(bet3);

                    if (parleyBet.Bet4Id != 0)
                    {
                        BetModel? bet4 = bets.Where(b => b.Id == parleyBet.Bet4Id).FirstOrDefault();

                        if (bet4 is not null)
                            parleyBet.Bets.Add(bet4);

                        if (parleyBet.Bet5Id != 0)
                        {
                            BetModel? bet5 = bets.Where(b => b.Id == parleyBet.Bet5Id).FirstOrDefault();
                            if (bet5 is not null)
                                parleyBet.Bets.Add(bet5);
                        }
                    }
                }
            }

            parleyBet.Bets.PopulateBetModelsWithGamesAndTeams(games, teams);
        }

        return parleyBetsToPopulate;
    }
}

#nullable restore
