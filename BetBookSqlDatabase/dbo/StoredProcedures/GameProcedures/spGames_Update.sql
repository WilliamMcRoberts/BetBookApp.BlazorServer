CREATE PROCEDURE [dbo].[spGames_Update]
	@Id int,
	@HomeTeamId int,
	@AwayTeamId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@HomeTeamFinalScore float,
	@AwayTeamFinalScore float,
    @GameWinnerId int,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20),
    @ScoreId int,
    @DateOfGameOnly nvarchar(50),
    @TimeOfGameOnly nvarchar(50)
AS
begin
    update dbo.Games
	set HomeTeamId = @HomeTeamId, 
        AwayTeamId = @AwayTeamId, 
        Stadium = @Stadium, 
        PointSpread = @PointSpread, 
        HomeTeamFinalScore = @HomeTeamFinalScore, 
        AwayTeamFinalScore = @AwayTeamFinalScore,
        GameWinnerId = @GameWinnerId,
        SeasonType = @SeasonType, 
        DateOfGame = @DateOfGame, 
        GameStatus = @GameStatus,
        ScoreId = @ScoreId,
        DateOfGameOnly = @DateOfGameOnly,
        TimeOfGameOnly = @TimeOfGameOnly
	where Id = @Id;
end
