using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;
public interface IAvailableGames
{
    Task<TeamRecordModel[]> GetTeamRecords(List<BasicGameModel> basicGames);
    Task<List<BasicGameModel>> PopulateBasicGameModelList(List<GameModel> games);
}