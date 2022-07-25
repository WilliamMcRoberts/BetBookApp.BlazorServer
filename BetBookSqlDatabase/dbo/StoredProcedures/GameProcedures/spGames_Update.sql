﻿CREATE PROCEDURE [dbo].[spGames_Update]
	@Id int,
	@HomeTeamId int,
	@AwayTeamId int,
	@FavoriteId int,
	@UnderdogId int,
	@Stadium nvarchar(50),
	@PointSpread float,
	@FavoriteFinalScore float,
	@UnderdogFinalScore float,
    @GameWinnerId int,
	@SeasonType nvarchar(4),
	@DateOfGame date,
	@GameStatus nvarchar(20)
AS
begin
    update dbo.Games
	set HomeTeamId = @HomeTeamId, 
        AwayTeamId = @AwayTeamId, 
        FavoriteId = @FavoriteId, 
        UnderdogId = @UnderdogId, 
        Stadium = @Stadium, 
        PointSpread = @PointSpread, 
        FavoriteFinalScore = @FavoriteFinalScore, 
        UnderdogFinalScore = @UnderdogFinalScore, 
        GameWinnerId = @GameWinnerId,
        SeasonType = @SeasonType, 
        DateOfGame = @DateOfGame, 
        GameStatus = @GameStatus
	where Id = @Id;
end
