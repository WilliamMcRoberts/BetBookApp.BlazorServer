
using BetBookData.Models;

namespace BetBookData.Helpers;
public static class StatusCheckHelpers
{
    public static bool CheckIfParleyBetLoser(this ParleyBetModel parleyBet)
    {
        foreach (BetModel bet in parleyBet.Bets)
        {
            if (bet.BetStatus == BetStatus.LOSER)
                return true;
        }

        return false;
    }

    public static bool CheckIfParleyBetPush(this ParleyBetModel parleyBet)
    {
        foreach (BetModel bet in parleyBet.Bets)
        {
            if (bet.BetStatus != BetStatus.PUSH)
                return false;
        }

        return true;
    }

    public static bool CheckIfParleyBetWinner(this ParleyBetModel parleyBet)
    {
        foreach (BetModel bet in parleyBet.Bets)
        {
            if (bet.BetStatus != BetStatus.WINNER)
                return false;
        }

        return true;
    }
}
