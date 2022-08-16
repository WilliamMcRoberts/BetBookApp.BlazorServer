CREATE PROCEDURE [dbo].[spBets_Update]
    @Id int,
	@BetAmount money,
	@BetPayout money,
	@BettorId int,
	@GameId int,
	@ChosenWinnerId int,
	@FinalWinnerId int,
	@BetStatus nvarchar(20),
	@PayoutStatus nvarchar(20),
    @PointSpread float
AS
begin
    update dbo.Bets
	set BetAmount = @BetAmount, 
        BetPayout = @BetPayout, 
        BettorId = @BettorId, 
        GameId = @GameId, 
        ChosenWinnerId = @ChosenWinnerId, 
        FinalWinnerId = @FinalWinnerId,
        BetStatus = @BetStatus, 
        PayoutStatus = @PayoutStatus,
        PointSpread = @PointSpread
	where Id = @Id;
end
