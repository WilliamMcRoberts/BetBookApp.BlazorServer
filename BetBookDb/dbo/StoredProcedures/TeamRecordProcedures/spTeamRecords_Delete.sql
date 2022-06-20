CREATE PROCEDURE [dbo].[spTeamRecords_Delete]
    @TeamId int
AS
begin
    delete
	from dbo.TeamRecords
	where TeamId = @TeamId;
end
