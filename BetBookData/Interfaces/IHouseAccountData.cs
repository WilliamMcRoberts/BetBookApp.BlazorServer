using BetBookData.Models;

namespace BetBookData.Interfaces;

/// <summary>
/// HouseAccountData interface
/// </summary>
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}
