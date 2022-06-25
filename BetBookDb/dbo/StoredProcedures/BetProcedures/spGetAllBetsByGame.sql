CREATE PROCEDURE [dbo].[spGetAllByGame]
	@GameId int
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
	where GameId = @GameId;
end
