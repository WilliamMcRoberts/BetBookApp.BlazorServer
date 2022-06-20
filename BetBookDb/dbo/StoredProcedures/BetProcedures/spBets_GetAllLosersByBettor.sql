CREATE PROCEDURE [dbo].[spBets_GetAllLosersByBettor]
    @BettorId int
AS
begin
    select Id,
           BetAmount,
           BetPayout,
           BettorId,
           GameId,
           ChosenWinnerId,
           FinalWinnerId, 
           BetStatus,
           PayoutStatus
	from dbo.Bets
	where BettorId = @BettorId and BetStatus = 'LOSERS';
end
