
using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;

namespace BetBookUI.Helpers;

#nullable disable

public static class AuthenticationStateProviderHelpers
{

    public static async Task<UserModel> GetUserFromAuth(
        this AuthenticationStateProvider provider, IMediator mediator)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        return await mediator.Send(new GetUserByObjectIdQuery(objectId));
    }


    public static async Task LoadAndVerifyUser(
        this AuthenticationStateProvider provider, 
        UserModel loggedInUser, IMediator mediator)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        if (string.IsNullOrWhiteSpace(objectId) == false)
        {
            loggedInUser = await mediator.Send(new GetUserByObjectIdQuery(objectId)) ?? new();

            string firstName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("givenname"))?.Value;
            string lastName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("surname"))?.Value;
            string displayName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Equals("name"))?.Value;
            string emailAddress = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("email"))?.Value;

            bool isDirty = false;

            if (objectId.Equals(loggedInUser.ObjectIdentifier) == false)
            {
                isDirty = true;
                loggedInUser.ObjectIdentifier = objectId;
            }
            if (firstName.Equals(loggedInUser.FirstName) == false)
            {
                isDirty = true;
                loggedInUser.FirstName = firstName;
            }

            if (lastName.Equals(loggedInUser.LastName) == false)
            {
                isDirty = true;
                loggedInUser.LastName = lastName;
            }

            if (displayName.Equals(loggedInUser.DisplayName) == false)
            {
                isDirty = true;
                loggedInUser.DisplayName = displayName;
            }

            if (emailAddress.Equals(loggedInUser.EmailAddress) == false)
            {
                isDirty = true;
                loggedInUser.EmailAddress = emailAddress;
            }

            if (isDirty)
            {
                if (loggedInUser.Id == 0)
                {
                    // New user recieves 10,000 in account
                    loggedInUser.AccountBalance = 10000;

                    await mediator.Send(new InsertUserCommand(loggedInUser));
                }

                else
                    await mediator.Send(new UpdateUserCommand(loggedInUser));
            }
        }
    }
}

#nullable enable
