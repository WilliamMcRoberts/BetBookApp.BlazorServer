CREATE PROCEDURE [dbo].[spUsers_GetAll]
AS
begin
    select Id, FirstName, LastName, EmailAddress, ObjectIdentifier, DisplayName, AccountBalance
	from dbo.Users;
end
