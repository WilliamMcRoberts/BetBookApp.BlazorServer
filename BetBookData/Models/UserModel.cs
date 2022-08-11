
namespace BetBookData.Models;

#nullable enable

/// <summary>
/// User model
/// </summary>
public class UserModel
{
    // ID for User
    public int Id { get; set; }

    // First name of user
    public string FirstName { get; set; } = string.Empty;

    // Last name of user
    public string LastName { get; set; } = string.Empty;

    // Email address of user
    public string EmailAddress { get; set; } = string.Empty;

    // Object identifier from Azure AD B2C
    public string ObjectIdentifier { get; set; } = string.Empty;

    // Display name of user
    public string DisplayName { get; set; } = string.Empty;

    // Account balance of user
    public decimal AccountBalance { get; set; }
}

#nullable restore
