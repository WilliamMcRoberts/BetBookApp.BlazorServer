CREATE PROCEDURE [dbo].[spTeamRecords_Update]
    @TeamId int,
	@Wins nvarchar(50),
	@Losses nvarchar(50),
	@Draws nvarchar(50)
AS
begin
    update dbo.TeamRecords
	set Wins = @Wins, Losses = @Losses, Draws = @Draws
	where TeamId = @TeamId;
end
