CREATE PROCEDURE [dbo].[spGames_GetAll]
AS
begin
    select Id, 
           HomeTeamId, 
           AwayTeamId, 
           Stadium, 
           PointSpread,
           HomeTeamFinalScore, 
           AwayTeamFinalScore, 
           GameWinnerId, 
           WeekNumber, 
           SeasonType, 
           DateOfGame, 
           GameStatus,
           ScoreId,
           DateOfGameOnly,
           TimeOfGameOnly
	from dbo.Games;
end
