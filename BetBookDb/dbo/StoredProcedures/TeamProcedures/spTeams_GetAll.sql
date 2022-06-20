CREATE PROCEDURE [dbo].[spTeams_GetAll]

AS
begin
    select Id, TeamName, City, Stadium, WinCount, LossCount, DrawCount
	from dbo.Teams;
end
