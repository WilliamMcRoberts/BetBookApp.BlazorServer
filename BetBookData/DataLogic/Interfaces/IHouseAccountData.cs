﻿using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;

/// <summary>
/// HouseAccountData interface
/// </summary>
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}
