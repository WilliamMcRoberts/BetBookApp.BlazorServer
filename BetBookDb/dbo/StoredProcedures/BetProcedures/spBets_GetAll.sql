CREATE PROCEDURE [dbo].[spBets_GetAll]
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
	from dbo.Bets;
end
