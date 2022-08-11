using BetBookData.Models;

namespace BetBookData.Helpers;

#nullable enable

public static class PopulationHelpers
{
    public static List<GameModel> PopulateGameModelsWithTeams(
        this List<GameModel> games, IEnumerable<TeamModel> teams)
    {
        foreach (GameModel g in games)
        {
            g.HomeTeam = teams.Where(t => t.Id == g.HomeTeamId).FirstOrDefault()!;
            g.AwayTeam = teams.Where(t => t.Id == g.AwayTeamId).FirstOrDefault()!;

            g.Favorite = teams.Where(t => t.Id == g.FavoriteId).FirstOrDefault()!;
            g.Underdog = teams.Where(t => t.Id == g.UnderdogId).FirstOrDefault()!;

            if (g.GameWinnerId != 0)
                g.GameWinner = teams.Where(t => t.Id == g.GameWinnerId).FirstOrDefault()!;
        }

        return games;
    }

    public static List<BetModel> PopulateBetModelsWithGamesAndTeams(
        this List<BetModel> bets, IEnumerable<GameModel> games, 
            IEnumerable<TeamModel> teams)
    {
        foreach(BetModel b in bets)
        {

            b.Game = games.Where(g => g.Id == b?.GameId).ToList().PopulateGameModelsWithTeams(teams).FirstOrDefault();

            b.ChosenWinner = teams.Where(t => t.Id == b.ChosenWinnerId).FirstOrDefault();

            b.FinalWinner = 
                b.FinalWinnerId != 0 ? teams.Where(t => t.Id == b.FinalWinnerId).FirstOrDefault() : null;
        }

        return bets;
    }

    public static List<ParleyBetModel> PopulateParleyBetsWithBetsWithGamesAndTeams(
        this List<ParleyBetModel> parleyBets, IEnumerable<GameModel> games, 
            IEnumerable<TeamModel> teams, IEnumerable<BetModel> bets)
    {
        foreach(ParleyBetModel parleyBet in parleyBets)
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

        return parleyBets;
    }
}

#nullable restore
