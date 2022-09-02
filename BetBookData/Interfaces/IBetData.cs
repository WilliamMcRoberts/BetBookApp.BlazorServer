using BetBookData.Models;

namespace BetBookData.Interfaces;

public interface IBetData
{
    Task<IEnumerable<BetModel>> GetBetsOnCurrentGame(int _gameId);
    Task<IEnumerable<BetModel>> GetBettorBetsUnpaid(int _bettorId);
    Task<int> InsertBet(BetModel _bet);
    Task<bool> PayoutUnpaidPushBets(decimal _totalPendingRefund, int _userId);
    Task<bool> PayoutUnpaidWinningBets(decimal _totalPendingPayout, int _userId);
    Task UpdateBet(BetModel bet);
}