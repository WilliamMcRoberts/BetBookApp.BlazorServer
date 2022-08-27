
namespace BetBookDbAccess;

#nullable enable

public interface ISqlConnection
{
    Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionId = "BetBookDB");
    Task SaveData<T>(
        string storedProcedure, T parameters, string connectionId = "BetBookDB");
}

#nullable restore
