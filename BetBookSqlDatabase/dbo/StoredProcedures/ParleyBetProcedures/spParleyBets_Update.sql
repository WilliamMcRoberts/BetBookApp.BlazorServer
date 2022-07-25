CREATE PROCEDURE [dbo].[spParleyBets_Update]
    @Id int,
	@Bet1Id int,
	@Bet2Id int,
	@Bet3Id int,
	@Bet4Id int,
	@Bet5Id int,
	@BettorId int,
	@BetAmount money,
	@BetPayout money,
	@ParleyBetStatus nvarchar(20),
	@ParleyPayoutStatus nvarchar(20)
AS
begin
    update dbo.ParleyBets

	set Bet1Id = @Bet1Id,
        Bet2Id = @Bet2Id,
        Bet3Id = @Bet3Id,
        Bet4Id = @Bet4Id,
        Bet5Id = @Bet5Id, 
        BettorId = @BettorId,
        BetAmount = @BetAmount, 
        BetPayout = @BetPayout, 
        ParleyBetStatus = @ParleyBetStatus,
        ParleyPayoutStatus = @ParleyPayoutStatus

	where Id = @Id;
end
