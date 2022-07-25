CREATE PROCEDURE [dbo].[spGames_UpdateDraw]
    @Id int,
	@HomeTeamId int,
	@AwayTeamId int,
	@FavoriteId int,
	@UnderdogId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@FavoriteFinalScore float,
	@UnderdogFinalScore float,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20)
AS
BEGIN
    update dbo.Games
	set HomeTeamId = @HomeTeamId, 
        AwayTeamId = @AwayTeamId, 
        FavoriteId = @FavoriteId, 
        UnderdogId = @UnderdogId, 
        Stadium = @Stadium, 
        PointSpread = @PointSpread, 
        FavoriteFinalScore = @FavoriteFinalScore, 
        UnderdogFinalScore = @UnderdogFinalScore,
        SeasonType = @SeasonType, 
        DateOfGame = @DateOfGame, 
        GameStatus = @GameStatus
	where Id = @Id;
END
