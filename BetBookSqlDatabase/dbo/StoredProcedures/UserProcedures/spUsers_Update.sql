CREATE PROCEDURE [dbo].[spUsers_Update]
	@Id int,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@EmailAddress nvarchar(100),
	@ObjectIdentifier nvarchar(100),
	@DisplayName nvarchar(50)
AS
begin
    update dbo.Users
	set FirstName = @FirstName, LastName = @LastName, EmailAddress = @EmailAddress, ObjectIdentifier = @ObjectIdentifier, DisplayName = @DisplayName
	where Id =@Id;
end
