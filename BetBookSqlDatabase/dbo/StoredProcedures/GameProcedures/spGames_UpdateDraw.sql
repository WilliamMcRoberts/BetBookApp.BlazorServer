CREATE PROCEDURE [dbo].[spGames_UpdateDraw]
    @Id int,
	@HomeTeamId int,
	@AwayTeamId int,
	@FavoriteId int,
	@UnderdogId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@HomeTeamFinalScore float,
	@AwayTeamFinalScore float,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20),
    @ScoreId int,
    @DateOfGameOnly nvarchar(50),
    @TimeOfGameOnly nvarchar(50)
AS
BEGIN
    update dbo.Games
	set HomeTeamId = @HomeTeamId, 
        AwayTeamId = @AwayTeamId, 
        FavoriteId = @FavoriteId, 
        UnderdogId = @UnderdogId, 
        Stadium = @Stadium, 
        PointSpread = @PointSpread, 
        HomeTeamFinalScore = @HomeTeamFinalScore, 
        AwayTeamFinalScore = @AwayTeamFinalScore,
        SeasonType = @SeasonType, 
        DateOfGame = @DateOfGame, 
        GameStatus = @GameStatus,
        ScoreId = @ScoreId,
        DateOfGameOnly = @DateOfGameOnly,
        TimeOfGameOnly = @TimeOfGameOnly
	where Id = @Id;
END
