using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface ITransactionService
{
    Task<bool> CreateBetTransaction(UserModel user, BetModel bet);
    Task<bool> CreateParleyBetTransaction(UserModel user, ParleyBetModel parleyBet);
    Task<bool> PayoutBetsTransaction(UserModel user, List<BetModel> bettorBetsUnpaid, decimal totalPendingPayout);
    Task<bool> PayoutParleyBetsTransaction(UserModel user, List<ParleyBetModel> bettorParleyBetsUnpaid, decimal totalPendingParleyPayout);
}