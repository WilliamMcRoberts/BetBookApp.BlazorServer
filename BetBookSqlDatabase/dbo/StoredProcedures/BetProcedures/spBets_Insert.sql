CREATE PROCEDURE [dbo].[spBets_Insert]
    @BetAmount money,
	@BetPayout money,
	@BettorId int,
	@GameId int,
	@ChosenWinnerId int,
	@BetStatus nvarchar(20),
	@PayoutStatus nvarchar(20),
    @PointSpread float,
    @Id int = 0 output
AS
begin
    insert into dbo.Bets (BetAmount, 
                          BetPayout, 
                          BettorId, 
                          GameId, 
                          ChosenWinnerId, 
                          BetStatus,
                          PayoutStatus,
                          PointSpread)
	values (@BetAmount, 
            @BetPayout,
            @BettorId,
            @GameId, 
            @ChosenWinnerId,
            @BetStatus,
            @PayoutStatus,
            @PointSpread);

    select @Id = SCOPE_IDENTITY();
end
