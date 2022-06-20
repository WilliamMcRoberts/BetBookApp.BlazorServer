CREATE PROCEDURE [dbo].[spGames_GetAllNotStarted]

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
	where GameStatus = 'NOT_STARTED';
end
