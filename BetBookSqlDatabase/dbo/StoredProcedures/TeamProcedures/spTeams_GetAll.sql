CREATE PROCEDURE [dbo].[spTeams_GetAll]

AS
begin
    select Id, TeamName, City, Stadium, Wins, Losses, Draws, Symbol, Division, Conference
	from dbo.Teams;
end
