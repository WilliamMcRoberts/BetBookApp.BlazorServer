CREATE PROCEDURE [dbo].[spGames_Get]
    @Id int
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
	from dbo.Games
	where Id = @Id;
end
