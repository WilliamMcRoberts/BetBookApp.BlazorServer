using BetBookData.Models;

namespace BetBookData.Interfaces;

#nullable enable

/// <summary>
/// HouseAccountData interface
/// </summary>
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}

#nullable restore
