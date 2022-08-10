using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookMinApi.Api;

public static class UsersApi
{
    public static void ConfigureUsersApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Users", GetUsers).WithName("GetUsers");
        app.MapGet("/Users/{id}", GetUser).WithName("GetUserById");
        app.MapGet("/Users/objectIdentifier", GetUserFromAuthentication).WithName("GetUserByObjectId");
        app.MapPost("/Users", InsertUser).WithName("InsertUser");
        app.MapPut("/Users", UpdateUser).WithName("UpdateUser");
        app.MapPut("/Users/AccountBalance", UpdateUserAccountBalance).WithName("UpdateUserAccountBalance");
        app.MapDelete("/Users/{id}", DeleteUser).WithName("DeleteUser");
    }

    public static async Task<IResult> GetUsers(IUserData data)
    {
        try
        {
            return Results.Ok(await data.GetUsers());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetUser(int id, IUserData data)
    {
        try
        {
            return Results.Ok(await data.GetUser(id));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetUserFromAuthentication(string objectIdentifier, IUserData data)
    {
        try
        {
            return Results.Ok(await data.GetUserFromAuthentication(objectIdentifier));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertUser(UserModel user, IUserData data)
    {
        try
        {
            await data.InsertUser(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(UserModel user, IUserData data)
    {
        try
        {
            await data.UpdateUser(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUserAccountBalance(UserModel user, IUserData data)
    {
        try
        {
            await data.UpdateUserAccountBalance(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteUser(int id, IUserData data)
    {
        try
        {
            await data.DeleteUser(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
