
namespace BetBookData.Models;

/// <summary>
/// User model
/// </summary>
public class UserModel
{
    // ID for User
    public int Id { get; set; }

    // First name of user
    public string FirstName { get; set; }

    // Last name of user
    public string LastName { get; set; }

    // Email address of user
    public string EmailAddress { get; set; }

    // Object identifier from Azure AD B2C
    public string ObjectIdentifier { get; set; }

    // Display name of user
    public string DisplayName { get; set; }

    // Account balance of user
    public decimal AccountBalance { get; set; }
}
