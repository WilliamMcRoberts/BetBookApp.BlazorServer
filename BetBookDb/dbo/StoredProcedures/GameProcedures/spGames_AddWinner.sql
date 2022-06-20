CREATE PROCEDURE [dbo].[spGames_AddWinner]
    @Id int,
	@GameWinnerId int
AS
begin
    update dbo.Games
	set GameWinnerId = @GameWinnerId
	where Id = @Id;
end
