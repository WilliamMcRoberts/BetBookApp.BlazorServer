using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BetBookDataLogic.DbAccess;
public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;

    public SqlConnection(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Method for data access to sql server database (Load)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);

    }

    /// <summary>
    /// Method for data access to sql server database (Save)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="storedProcedure"></param>
    /// <param name="parameters"></param>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task SaveData<T>(
        string storedProcedure, T parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
