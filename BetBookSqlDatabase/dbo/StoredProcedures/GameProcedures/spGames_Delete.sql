CREATE PROCEDURE [dbo].[spGames_Delete]
    @Id int
AS
begin
    delete
	from dbo.Games
	where Id = @Id;
end
