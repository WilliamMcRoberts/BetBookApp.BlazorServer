CREATE PROCEDURE [dbo].[spUsers_GetByObjectIdentifier]
	@ObjectIdentifier nvarchar(100)
AS
begin
    select Id, FirstName, LastName, EmailAddress, ObjectIdentifier, DisplayName, AccountBalance
	from dbo.Users
	where ObjectIdentifier = @ObjectIdentifier;
end
