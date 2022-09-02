
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
        this IMediator _mediator, AuthenticationStateProvider _provider)
    {
        var authState = await _provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        return await _mediator.Send(new GetUserByObjectIdQuery(objectId));
    }


    public static async Task LoadAndVerifyUser(
        this IMediator _mediator, AuthenticationStateProvider _provider, 
        UserModel _loggedInUser)
    {
        var authState = await _provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        if (string.IsNullOrWhiteSpace(objectId) == false)
        {
            _loggedInUser = await _mediator.Send(new GetUserByObjectIdQuery(objectId)) ?? new();

            string firstName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("givenname"))?.Value;
            string lastName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("surname"))?.Value;
            string displayName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Equals("name"))?.Value;
            string emailAddress = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("email"))?.Value;

            bool isDirty = false;

            if (objectId.Equals(_loggedInUser.ObjectIdentifier) == false)
            {
                isDirty = true;
                _loggedInUser.ObjectIdentifier = objectId;
            }
            if (firstName.Equals(_loggedInUser.FirstName) == false)
            {
                isDirty = true;
                _loggedInUser.FirstName = firstName;
            }

            if (lastName.Equals(_loggedInUser.LastName) == false)
            {
                isDirty = true;
                _loggedInUser.LastName = lastName;
            }

            if (displayName.Equals(_loggedInUser.DisplayName) == false)
            {
                isDirty = true;
                _loggedInUser.DisplayName = displayName;
            }

            if (emailAddress.Equals(_loggedInUser.EmailAddress) == false)
            {
                isDirty = true;
                _loggedInUser.EmailAddress = emailAddress;
            }

            if (isDirty)
            {
                if (_loggedInUser.Id == 0)
                {
                    // New user recieves 10,000 in account
                    _loggedInUser.AccountBalance = 10000;

                    await _mediator.Send(new InsertUserCommand(_loggedInUser));
                }

                else
                    await _mediator.Send(new UpdateUserCommand(_loggedInUser));
            }
        }
    }
}

#nullable enable
