﻿using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;

/// <summary>
/// TeamRecordData interface
/// </summary>
public interface ITeamRecordData
{
    Task DeleteTeamRecord(int teamId);
    Task<TeamRecordModel?> GetTeamRecord(int teamId);
    Task<IEnumerable<TeamRecordModel>> GetTeamRecords();
    Task InsertTeamRecord(int teamId);
    Task UpdateTeamRecord(TeamRecordModel teamRecord);
}
