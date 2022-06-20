CREATE PROCEDURE [dbo].[spGames_GetAllByDate]
    @DateOfGame date
AS
begin
    select Id, 
           HomeTeamId, 
           AwayTeamId, 
           FavoriteId, 
           UnderdogId, 
           Stadium, 
           PointSpread, 
           FavoriteFinalScore, 
           UnderdogFinalScore, 
           GameWinnerId, 
           WeekNumber, 
           SeasonType, 
           DateOfGame, 
           GameStatus
	from dbo.Games
	where DateOfGame = @DateOfGame;
end
