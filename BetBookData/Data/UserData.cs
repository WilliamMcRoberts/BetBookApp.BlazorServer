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

    public async Task<UserModel?> GetUserFromAuthentication(string objectIdentifier)
    {
        _logger.LogInformation(message: "Get User From Authentication Call / UserData");

        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_GetByObjectIdentifier", new
            {
                ObjectIdentifier = objectIdentifier
            });

      return results.FirstOrDefault();
    }

    public async Task InsertUser(UserModel user)
    {
        _logger.LogInformation(message: "Insert User Call / UserData");

        try
        {
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
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert User / UserData");
        }
    }

    public async Task UpdateUser(UserModel user)
    {
        _logger.LogInformation(message: "Update User Call / UserData");

        try
        {
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
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update User / UserData");
        }
    }
}

#nullable restore
