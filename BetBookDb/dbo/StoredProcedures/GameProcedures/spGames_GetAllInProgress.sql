CREATE PROCEDURE [dbo].[spGames_GetAllInProgress]
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
	where GameStatus = 'IN_PROGRESS';
end
