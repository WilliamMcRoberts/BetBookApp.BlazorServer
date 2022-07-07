using System.Data;
using BetBookData.Interfaces;
using BetBookDbAccess;
using BetBookData.Models;

namespace BetBookData.Data;
public class UserData : IUserData
{

    private readonly ISqlConnection _db;


    /// <summary>
    /// UserData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public UserData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spUsers_GetAll stored procedure to retrieve 
    /// all users in the database
    /// </summary>
    /// <returns>
    /// IEnumerable of UserModel represents all users in database
    /// </returns>
    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        return await _db.LoadData<UserModel, dynamic>(
        "dbo.spUsers_GetAll", new { });
    }

    /// <summary>
    /// Async method calls spUsers_Get stored procedure which retrieves one 
    /// user by user id
    /// </summary>
    /// <param name="id">
    /// int represents Id of user to retrieve from the database
    /// </param>
    /// <returns>
    /// UserModel represents the user from database
    /// </returns>
    public async Task<UserModel?> GetUser(int id)
    {
        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls spUsers_GetByObjectIdentifier stored procedure which retieves 
    /// one user from the database using the object identifier from Azure AD B2C
    /// </summary>
    /// <param name="objectIdentifier">
    /// string represents the object id of user from Azure Auth
    /// </param>
    /// <returns>
    /// UserModel represents user to retrieve from Azure Auth
    /// </returns>
    public async Task<UserModel?> GetUserFromAuthentication(string objectIdentifier)
    {
        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUsers_GetByObjectIdentifier", new
            {
                ObjectIdentifier = objectIdentifier
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spUsers_Insert stored procedure to insert one user 
    /// entry into the database
    /// </summary>
    /// <param name="user">
    /// UserModel rpresents a user to insert into the database
    /// </param>
    /// <returns></returns>
    public async Task InsertUser(UserModel user)
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

    /// <summary>
    /// Async method calls the spUsers_Update stored procedure to update a user
    /// </summary>
    /// <param name="user">
    /// UserModel represents a user to update in the database
    /// </param>
    /// <returns></returns>
    public async Task UpdateUser(UserModel user)
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

    /// <summary>
    /// Async method calls the spUsers_UpdateAccountBalance stored procedure which
    /// updates the account balance of a user
    /// </summary>
    /// <param name="user">
    /// UserModel represents a user of which the account balance is being updated
    /// </param>
    /// <returns></returns>
    public async Task UpdateUserAccountBalance(UserModel user)
    {
        await _db.SaveData(
        "dbo.spUsers_UpdateAccountBalance", new
        {
            user.Id,
            user.AccountBalance
        });
    }

    /// <summary>
    /// Async method calls the spUsers_Delete stored procedure which deletes one 
    /// user entry in the database
    /// </summary>
    /// <param name="id">
    /// int represents the Id of the user being deleted from database
    /// </param>
    /// <returns></returns>
    public async Task DeleteUser(int id)
    {
        await _db.SaveData(
        "dbo.spUsers_Delete", new
        {
            Id = id
        });
    }
}
