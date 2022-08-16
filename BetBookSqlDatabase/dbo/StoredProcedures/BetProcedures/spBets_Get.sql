CREATE PROCEDURE [dbo].[spBets_Get]
    @Id int
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
           PointSpread
	from dbo.Bets
	where Id = @Id;
end
