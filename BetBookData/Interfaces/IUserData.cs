using BetBookData.Models;

namespace BetBookData.Interfaces;

#nullable enable

public interface IUserData
{
    Task<UserModel> GetUserFromAuthentication(string objectIdentifier);
    Task InsertUser(UserModel user);
    Task UpdateUser(UserModel user);
}

#nullable restore