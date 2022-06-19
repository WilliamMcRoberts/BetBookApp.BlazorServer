using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data.Interfaces;
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}
