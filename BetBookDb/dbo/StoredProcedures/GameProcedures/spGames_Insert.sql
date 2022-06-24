CREATE PROCEDURE [dbo].[spGames_Insert]
	@HomeTeamId int,
	@AwayTeamId int,
	@FavoriteId int,
	@UnderdogId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@WeekNumber int,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20)
AS
begin
    insert into dbo.Games (HomeTeamId, 
                           AwayTeamId, 
                           FavoriteId, 
                           UnderdogId, 
                           Stadium, 
                           PointSpread, 
                           WeekNumber, 
                           SeasonType, 
                           DateOfGame, 
                           GameStatus)
	values (@HomeTeamId, 
            @AwayTeamId, 
            @FavoriteId, 
            @UnderdogId, 
            @Stadium, 
            @PointSpread, 
            @WeekNumber, 
            @SeasonType, 
            @DateOfGame, 
            @GameStatus);
end
