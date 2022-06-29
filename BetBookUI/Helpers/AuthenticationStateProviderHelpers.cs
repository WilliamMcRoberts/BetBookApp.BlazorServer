
using Microsoft.AspNetCore.Components.Authorization;

namespace BetBookUI.Helpers;

/// <summary>
///     Static helper class
/// </summary>
public static class AuthenticationStateProviderHelpers
{
    /// <summary>
    ///     Method to grab object identifier from Azure, 
    ///     then grab user from database
    /// </summary>
    /// <param name="provider">
    ///     AuthenticationStateProvider represents the identity provider
    /// </param>
    /// <param name="userData">
    ///     IUserData represents instance of UserData class
    /// </param>
    /// <returns>
    ///     UserModel current logged in user
    /// </returns>
    public static async Task<UserModel> GetUserFromAuth(
        this AuthenticationStateProvider provider, IUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;
        return await userData.GetUserFromAuthentication(objectId);
    }
}
