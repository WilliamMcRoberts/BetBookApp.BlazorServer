CREATE PROCEDURE [dbo].[spTeamRecords_GetAll]

AS
begin
    select TeamId, Wins, Losses, Draws
	from dbo.TeamRecords;
end
