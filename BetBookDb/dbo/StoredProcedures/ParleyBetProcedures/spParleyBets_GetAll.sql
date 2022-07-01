CREATE PROCEDURE [dbo].[spParleyBets_GetAll]

AS
begin
    select Id,
           Bet1Id,
           Bet1Id,
           Bet3Id,
           Bet4Id,
           Bet5Id,
           BettorId, 
           BetAmount, 
           BetPayout, 
           ParleyBetStatus, 
           ParleyPayoutStatus

	from dbo.ParleyBets;
end