CREATE PROCEDURE [dbo].[spTeams_Delete]
    @Id int
AS
begin
    delete
	from dbo.Teams
	where Id = @Id;
end
