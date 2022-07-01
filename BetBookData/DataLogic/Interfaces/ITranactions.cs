using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;

/// <summary>
/// Transactions interface
/// </summary>
public interface ITranactions
{
    Task CreateBetTransaction(UserModel user, HouseAccountModel houseAccount, BetModel bet);
    Task PayoutBetsTransaction(UserModel user, HouseAccountModel houseAccount, List<BetModel> bettorBetsUnpaid);
}
