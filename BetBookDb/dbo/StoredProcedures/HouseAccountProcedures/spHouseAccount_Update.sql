CREATE PROCEDURE [dbo].[spHouseAccount_Update]
	@AccountBalance money
AS
begin
    update dbo.HouseAccount
	set AccountBalance = @AccountBalance;
end
