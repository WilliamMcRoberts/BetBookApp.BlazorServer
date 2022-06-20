CREATE PROCEDURE [dbo].[spGames_GetAllByWeek]
    @WeekNumber int
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
	where WeekNumber = @WeekNumber;
end
