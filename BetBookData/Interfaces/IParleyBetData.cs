using BetBookData.Models;

namespace BetBookData.Interfaces;

public interface IParleyBetData
{
    Task<IEnumerable<ParleyBetModel>> GetInProgressParleyBets();
    Task<IEnumerable<ParleyBetModel>> GetBettorParleyBetsUnpaid(int _bettorId);
    Task<int> InsertParleyBet(ParleyBetModel _parleyBet);
    Task<bool> PayoutUnpaidPushParleyBets(decimal _totalPendingParleyRefund, int _userId);
    Task<bool> PayoutUnpaidWinningParleyBets(decimal _totalPendingParleyPayout, int _userId);
    Task UpdateParleyBet(ParleyBetModel parleyBet);
}