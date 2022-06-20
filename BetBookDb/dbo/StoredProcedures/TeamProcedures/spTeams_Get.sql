CREATE PROCEDURE [dbo].[spTeams_Get]
    @Id int
AS
begin
    select Id, TeamName, City, Stadium, WinCount, LossCount, DrawCount
	from dbo.Teams
	where Id = @Id;
end
