using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookDbAccess;
using Microsoft.Extensions.Logging;

namespace BetBookData.Data;

#nullable enable

public class UserData : IUserData
{

    private readonly ISqlConnection _db;
    private readonly ILogger<UserData> _logger;

    public UserData(ISqlConnection db, ILogger<UserData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        _logger.LogInformation(message: "Http Get / Get Users");

        return await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_GetAll", new { });
    }

    public async Task<UserModel?> GetUser(int id)
    {
        _logger.LogInformation(message: "Http Get / Get User By Id");

        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    public async Task<UserModel?> GetUserFromAuthentication(string objectIdentifier)
    {
        _logger.LogInformation(message: "Http Get / Get User By Object Identifier");

        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_GetByObjectIdentifier", new
            {
                ObjectIdentifier = objectIdentifier
            });

        return results.FirstOrDefault();
    }

    public async Task InsertUser(UserModel user)
    {
        _logger.LogInformation(message: "Http Post / Insert User");

        await _db.SaveData(
        "dbo.spUsers_Insert", new
        {
            user.FirstName,
            user.LastName,
            user.EmailAddress,
            user.ObjectIdentifier,
            user.DisplayName,
            user.AccountBalance
        });
    }

    public async Task UpdateUser(UserModel user)
    {
        _logger.LogInformation(message: "Http Put / Update User");

        await _db.SaveData(
        "dbo.spUsers_Update", new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.EmailAddress,
            user.ObjectIdentifier,
            user.DisplayName
        });
    }

    public async Task UpdateUserAccountBalance(UserModel user)
    {
        _logger.LogInformation(message: "Http Put / Update User Account Balance");

        await _db.SaveData(
        "dbo.spUsers_UpdateAccountBalance", new
        {
            user.Id,
            user.AccountBalance
        });
    }

    public async Task DeleteUser(int id)
    {
        _logger.LogInformation(message: "Http Delete / Delete User");

        await _db.SaveData(
        "dbo.spUsers_Delete", new
        {
            Id = id
        });
    }
}

#nullable restore
