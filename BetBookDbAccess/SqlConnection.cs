using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BetBookDbAccess;

#nullable enable

public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;
    private readonly ILogger<SqlConnection> _logger;

    public SqlConnection(IConfiguration config, ILogger<SqlConnection> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = 
            new System.Data.SqlClient.SqlConnection(
                _config.GetConnectionString(connectionId));

        _logger.LogInformation("Begin Sql Db Query...Load");

        return await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task SaveData<T>(
        string storedProcedure, T parameters, string connectionId = "BetBookDB")
    {
        using IDbConnection connection = 
            new System.Data.SqlClient.SqlConnection(
                _config.GetConnectionString(connectionId));

        _logger.LogInformation("Begin Sql Db Query...Save");

        await connection.ExecuteAsync(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}

#nullable restore
