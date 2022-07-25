CREATE PROCEDURE [dbo].[spParleyBets_Get]
    @Id int
AS
begin
    select Id,
           Bet1Id,
           Bet2Id,
           Bet3Id,
           Bet4Id,
           Bet5Id,
           BettorId, 
           BetAmount, 
           BetPayout, 
           ParleyBetStatus, 
           ParleyPayoutStatus

	from dbo.ParleyBets
    where Id = @Id;
end
