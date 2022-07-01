CREATE TABLE [dbo].[ParleyBets]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Bet1Id] INT NOT NULL, 
    [Bet2Id] INT NOT NULL, 
    [Bet3Id] INT NULL, 
    [Bet4Id] INT NULL, 
    [Bet5Id] INT NULL, 
    [BettorId] INT NOT NULL, 
    [BetAmount] MONEY NOT NULL, 
    [BetPayout] MONEY NOT NULL, 
    [ParleyBetStatus] NVARCHAR(20) NOT NULL, 
    [ParleyPayoutStatus] NVARCHAR(20) NOT NULL
)
