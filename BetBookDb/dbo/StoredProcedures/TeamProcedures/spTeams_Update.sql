CREATE PROCEDURE [dbo].[spTeams_Update]
    @Id int,
	@TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@WinCount int,
	@LossCount int,
	@DrawCount int
AS
begin
    update dbo.Teams
	set TeamName = @TeamName, City = @City, Stadium = @Stadium, WinCount = @WinCount, LossCount = @LossCount, DrawCount = @DrawCount
	where Id = @Id;
end
