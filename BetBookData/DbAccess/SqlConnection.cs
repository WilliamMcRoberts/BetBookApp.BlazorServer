using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BetBookData.DbAccess;

#nullable enable

public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;

    public SqlConnection(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = 
            new System.Data.SqlClient.SqlConnection(
                _config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

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

#nullable restore
