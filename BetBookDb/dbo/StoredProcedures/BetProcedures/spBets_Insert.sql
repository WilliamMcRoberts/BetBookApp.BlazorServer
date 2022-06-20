CREATE PROCEDURE [dbo].[spBets_Insert]
    @BetAmount money,
	@BetPayout money,
	@BettorId int,
	@GameId int,
	@ChosenWinnerId int,
	@BetStatus nvarchar(20),
	@PayoutStatus nvarchar(20)
AS
begin
    insert into dbo.Bets (BetAmount, 
                          BetPayout, 
                          BettorId, 
                          GameId, 
                          ChosenWinnerId, 
                          BetStatus,
                          PayoutStatus)
	values (@BetAmount, 
            @BetPayout,
            @BettorId,
            @GameId, 
            @ChosenWinnerId,
            @BetStatus,
            @PayoutStatus);
end
