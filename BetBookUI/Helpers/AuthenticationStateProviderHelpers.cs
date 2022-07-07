
using Microsoft.AspNetCore.Components.Authorization;

namespace BetBookUI.Helpers;

/// <summary>
/// Static Auth Provider helper class
/// </summary>
public static class AuthenticationStateProviderHelpers
{
    /// <summary>
    /// Static method to grab object identifier from Azure, 
    /// then grab user from database
    /// </summary>
    /// <param name="provider">
    /// AuthenticationStateProvider represents the identity provider
    /// </param>
    /// <param name="userData">
    /// IUserData represents instance of UserData class
    /// </param>
    /// <returns>
    /// UserModel current logged in user
    /// </returns>
    public static async Task<UserModel> GetUserFromAuth(
        this AuthenticationStateProvider provider, IUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string? objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        return await userData.GetUserFromAuthentication(objectId);
    }

    /// <summary>
    /// Async method that grabs logged in user's info from Azure AD B2C and checks if they have a sql User entry and if not calls the spUsers_Insert stored procedure
    /// If user has an entry it checks for any new info and if so calls the spUsers_Update stored procedure
    /// </summary>
    /// <returns></returns>
    public static async Task LoadAndVerifyUser(
        this AuthenticationStateProvider provider, UserModel loggedInUser,
            IUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string? objectIdentifier = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;

        if (string.IsNullOrWhiteSpace(objectIdentifier) == false)
        {
            loggedInUser ??= new();

            string? firstName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("givenname"))?.Value;
            string? lastName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("surname"))?.Value;
            string? displayName = authState.User.Claims.FirstOrDefault(
                c => c.Type.Equals("name"))?.Value;
            string? emailAddress = authState.User.Claims.FirstOrDefault(
                c => c.Type.Contains("email"))?.Value;

            bool isDirty = false;

            if (objectIdentifier.Equals(loggedInUser.ObjectIdentifier) == false)
            {
                isDirty = true;
                loggedInUser.ObjectIdentifier = objectIdentifier;
            }
            if (firstName?.Equals(loggedInUser.FirstName) == false)
            {
                isDirty = true;
                loggedInUser.FirstName = firstName;
            }

            if (lastName?.Equals(loggedInUser.LastName) == false)
            {
                isDirty = true;
                loggedInUser.LastName = lastName;
            }

            if (displayName?.Equals(loggedInUser.DisplayName) == false)
            {
                isDirty = true;
                loggedInUser.DisplayName = displayName;
            }

            if (emailAddress?.Equals(loggedInUser.EmailAddress) == false)
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

                    await userData.InsertUser(loggedInUser);
                }

                else
                    await userData.UpdateUser(loggedInUser);
            }
        }
    }
}


