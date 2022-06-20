CREATE PROCEDURE [dbo].[spGetAllWinnersUnpaid]

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
	where BetStatus = 'WINNER' and PayoutStatus = 'UNPAID';
end
