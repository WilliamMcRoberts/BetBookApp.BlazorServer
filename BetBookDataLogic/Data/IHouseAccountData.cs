using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}
