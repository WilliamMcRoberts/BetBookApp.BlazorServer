﻿CREATE PROCEDURE [dbo].[spTeams_Get]
    @Id int
AS
begin
    select Id, TeamName, City, Stadium, Wins, Losses, Draws, Symbol
	from dbo.Teams
	where Id = @Id;
end
