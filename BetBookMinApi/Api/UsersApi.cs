using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class UsersApi
{
    public static void ConfigureUsersApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Users/objectIdentifier", GetUserFromAuthentication).WithName("GetUserByObjectId");
        app.MapPost("/Users", InsertUser).WithName("InsertUser");
        app.MapPut("/Users", UpdateUser).WithName("UpdateUser");
        app.MapPut("/Users/AccountBalance", UpdateUserAccountBalance).WithName("UpdateUserAccountBalance");
    }

    private static async Task<IResult> GetUserFromAuthentication(
        string objectIdentifier, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new GetUserByObjectIdQuery(objectIdentifier)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(UsersApi));
            logger.LogInformation(ex, "Exception On Get User From Authentication");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertUser(
        UserModel user, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new InsertUserCommand(user)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(UsersApi));
            logger.LogInformation(ex, "Exception On Insert User");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(
        UserModel user, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateUserCommand(user)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(UsersApi));
            logger.LogInformation(ex, "Exception On Update User");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUserAccountBalance(
        UserModel user, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateUserAccountBalanceCommand(user)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(UsersApi));
            logger.LogInformation(ex, "Exception On Update User Account Balance");

            return Results.Problem(ex.Message);
        }
    }
}
