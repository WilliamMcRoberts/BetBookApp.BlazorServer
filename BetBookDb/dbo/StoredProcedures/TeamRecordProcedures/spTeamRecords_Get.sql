CREATE PROCEDURE [dbo].[spTeamRecords_Get]
    @TeamId int
AS
begin
    select TeamId, Wins, Losses, Draws
	from dbo.TeamRecords
	where TeamId = @TeamId;
end
