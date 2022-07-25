CREATE PROCEDURE [dbo].[spUsers_UpdateAccountBalance]
	@Id int,
	@AccountBalance money
AS
begin
    update dbo.Users
	set AccountBalance = @AccountBalance
	where Id = @Id;
end
