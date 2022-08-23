using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IParleyBetData
{
    Task DeleteParleyBet(int id);
    Task<ParleyBetModel> GetParleyBet(int parleyBetId);
    Task<IEnumerable<ParleyBetModel>> GetParleyBets();
    Task InsertParleyBet(ParleyBetModel parleyBet);
    Task UpdateParleyBet(ParleyBetModel parleyBet);
}