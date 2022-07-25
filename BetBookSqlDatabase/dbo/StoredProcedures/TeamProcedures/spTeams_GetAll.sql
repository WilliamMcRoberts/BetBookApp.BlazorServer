CREATE PROCEDURE [dbo].[spTeams_GetAll]

AS
begin
    select Id, TeamName, City, Stadium, Wins, Losses, Draws
	from dbo.Teams;
end
