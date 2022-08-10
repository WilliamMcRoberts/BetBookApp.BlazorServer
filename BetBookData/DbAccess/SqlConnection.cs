using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BetBookData.DbAccess;
public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;

    /// <summary>
    /// SqlConnection Constructor
    /// </summary>
    /// <param name="config">
    /// IConfiguration represents configuration class interface
    /// </param>
    public SqlConnection(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Async method to retrieve data from database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="storedProcedure">
    /// string represents the stored procedure being called
    /// </param>
    /// <param name="parameters">
    /// dynamic represents the parameters being used in the stored procedure being called
    /// </param>
    /// <param name="connectionId">
    /// string represents the key for the connection string
    /// </param>
    /// <returns>
    /// IEnumerable of dynamic representing the objects being retrieved from the database
    /// </returns>
    public async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = 
            new System.Data.SqlClient.SqlConnection(
                _config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    /// <summary>
    /// Async method to save data to database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="storedProcedure">
    /// string represents the stored procedure being called 
    /// </param>
    /// <param name="parameters">
    /// dynamic represents the parameters being used in the stored procedure being called
    /// </param>
    /// <param name="connectionId">
    /// string represents the key for the connection string
    /// </param>
    /// <returns></returns>
    public async Task SaveData<T>(
        string storedProcedure, T parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = 
            new System.Data.SqlClient.SqlConnection(
                _config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
