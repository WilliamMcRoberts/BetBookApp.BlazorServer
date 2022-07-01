CREATE PROCEDURE [dbo].[spParleyBets_Insert]
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
    insert into dbo.ParleyBets (Bet1Id, 
                                Bet2Id, 
                                Bet3Id,
                                Bet4Id, 
                                Bet5Id, 
                                BettorId, 
                                BetAmount, 
                                BetPayout, 
                                ParleyBetStatus,
                                ParleyPayoutStatus)

	values (@Bet1Id,
            @Bet2Id,
            @Bet3Id,
            @Bet4Id,
            @Bet5Id,
            @BettorId,
            @BetAmount, 
            @BetPayout,
            @ParleyBetStatus,
            @ParleyPayoutStatus);
end
