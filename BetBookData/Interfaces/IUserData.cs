using BetBookData.Models;

namespace BetBookData.Interfaces;

#nullable enable

/// <summary>
/// UserData interface
/// </summary>
public interface IUserData
{
    Task DeleteUser(int id);
    Task<UserModel?> GetUser(int id);
    Task<UserModel?> GetUserFromAuthentication(string objectIdentifier);
    Task<IEnumerable<UserModel>> GetUsers();
    Task InsertUser(UserModel user);
    Task UpdateUser(UserModel user);
    Task UpdateUserAccountBalance(UserModel user);
}

#nullable restore
