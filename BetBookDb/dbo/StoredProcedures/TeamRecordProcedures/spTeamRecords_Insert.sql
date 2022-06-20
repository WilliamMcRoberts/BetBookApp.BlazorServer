CREATE PROCEDURE [dbo].[spTeamRecords_Insert]
    @TeamId int,
	@Wins nvarchar(50),
	@Losses nvarchar(50),
	@Draws nvarchar(50)
AS
begin
    insert into dbo.TeamRecords (TeamId, Wins, Losses, Draws)
	values (@TeamId, @Wins, @Losses, @Draws);
end
