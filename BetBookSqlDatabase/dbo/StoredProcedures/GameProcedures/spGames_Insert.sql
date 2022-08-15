CREATE PROCEDURE [dbo].[spGames_Insert]
	@HomeTeamId int,
	@AwayTeamId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@WeekNumber int,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20),
    @ScoreId int,
    @DateOfGameOnly nvarchar(50),
    @TimeOfGameOnly nvarchar(50)
AS
begin
    insert into dbo.Games (HomeTeamId, 
                           AwayTeamId, 
                           Stadium, 
                           PointSpread, 
                           WeekNumber, 
                           SeasonType, 
                           DateOfGame, 
                           GameStatus,
                           ScoreId,
                           DateOfGameOnly,
                           TimeOfGameOnly)
	values (@HomeTeamId, 
            @AwayTeamId,  
            @Stadium, 
            @PointSpread, 
            @WeekNumber, 
            @SeasonType, 
            @DateOfGame, 
            @GameStatus,
            @ScoreId,
            @DateOfGameOnly,
            @TimeOfGameOnly);
end
