using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}
