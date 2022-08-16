CREATE TABLE [dbo].[Bets]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BetAmount] MONEY NOT NULL, 
    [BetPayout] MONEY NOT NULL, 
    [BettorId] INT NOT NULL, 
    [GameId] INT NOT NULL, 
    [ChosenWinnerId] INT NOT NULL, 
    [FinalWinnerId] INT NULL, 
    [BetStatus] NVARCHAR(20) NOT NULL,
    [PayoutStatus] NVARCHAR(20) NOT NULL, 
    [PointSpread] FLOAT NOT NULL, 
    FOREIGN KEY (BettorId) REFERENCES Users(Id),
    FOREIGN KEY (GameId) REFERENCES Games(Id),
    FOREIGN KEY (ChosenWinnerId) REFERENCES Teams(Id),
    FOREIGN KEY (FinalWinnerId) REFERENCES Teams(Id),
)
