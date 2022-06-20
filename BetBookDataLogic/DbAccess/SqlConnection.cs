using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BetBookDataLogic.DbAccess;
public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;

    /// <summary>
    /// SqlConnection Constructor
    /// </summary>
    /// <param name="config"></param>
    public SqlConnection(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Method to retrieve data from database
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
    /// Method to save data to database
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
