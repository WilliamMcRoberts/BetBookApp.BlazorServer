using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public class UserData : IUserData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// Constructor for UserData
    /// </summary>
    /// <param name="db"></param>
    public UserData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Method calls the spUsers_GetAll stored procedure to retrieve 
    /// all users in the database
    /// </summary>
    /// <returns>IEnumerable of UserModel</returns>
    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        return await _db.LoadData<UserModel, dynamic>(
        "dbo.spUsers_GetAll", new { });
    }

    /// <summary>
    /// Method calls spUsers_Get stored procedure which retrieves one 
    /// user by user id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>UserModel</returns>
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
    /// Method calls spUsers_GetByObjectIdentifier stored procedure which retieves 
    /// one user from the database using the object identifier from Azure AD B2C
    /// </summary>
    /// <param name="objectIdentifier"></param>
    /// <returns></returns>
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
    /// Method calls the spUsers_Insert stored procedure to insert one user 
    /// entry into the database
    /// </summary>
    /// <param name="user"></param>
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
    /// Method calls the spUsers_Update stored procedure to update a user
    /// </summary>
    /// <param name="user"></param>
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
    /// Method calls the spUsers_UpdateAccountBalance stored procedure which
    /// updates the account balance of a user
    /// </summary>
    /// <param name="user"></param>
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
    /// Method calls the spUsers_Delete stored procedure which deletes one 
    /// user entry in the database
    /// </summary>
    /// <param name="id"></param>
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
