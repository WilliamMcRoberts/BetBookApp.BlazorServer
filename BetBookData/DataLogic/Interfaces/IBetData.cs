﻿using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;
public interface IBetData
{
    Task DeleteBet(int id);
    Task<IEnumerable<BetModel>> GetAllBetsOnGame(int gameId);
    Task<IEnumerable<BetModel>> GetAllBettorBets(int id);
    Task<IEnumerable<BetModel>> GetAllBettorInProgressBets(int id);
    Task<IEnumerable<BetModel>> GetAllBettorLosingBets(int id);
    Task<IEnumerable<BetModel>> GetAllBettorWinningBets(int id);
    Task<BetModel?> GetBet(int id);
    Task<IEnumerable<BetModel>> GetBets();
    Task InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}