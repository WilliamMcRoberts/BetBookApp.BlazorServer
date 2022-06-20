CREATE PROCEDURE [dbo].[spUsers_Get]
    @Id int
AS
begin
    select Id, FirstName, LastName, EmailAddress, ObjectIdentifier, DisplayName, AccountBalance
	from dbo.Users
	where Id = @Id;
end
