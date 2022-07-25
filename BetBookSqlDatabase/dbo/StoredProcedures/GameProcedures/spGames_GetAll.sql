CREATE PROCEDURE [dbo].[spGames_GetAll]
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
	from dbo.Games;
end
