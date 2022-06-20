CREATE PROCEDURE [dbo].[spUsers_Insert]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@EmailAddress nvarchar(100),
	@ObjectIdentifier nvarchar(100),
	@DisplayName nvarchar(50),
	@AccountBalance money
AS
begin
    insert into dbo.Users (FirstName, LastName, EmailAddress, ObjectIdentifier, DisplayName, AccountBalance)
	values (@FirstName, @LastName, @EmailAddress, @ObjectIdentifier, @DisplayName, @AccountBalance);
end
