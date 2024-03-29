﻿CREATE TABLE [dbo].[Games]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HomeTeamId] INT NOT NULL, 
    [AwayTeamId] INT NOT NULL, 
    [Stadium] NVARCHAR(50) NOT NULL, 
    [PointSpread] FLOAT NULL, 
    [HomeTeamFinalScore] FLOAT NULL, 
    [AwayTeamFinalScore] FLOAT NULL, 
    [GameWinnerId] INT NULL, 
    [WeekNumber] INT NOT NULL, 
    [Season] NVARCHAR(4) NOT NULL, 
    [DateOfGame] DATE NOT NULL, 
    [GameStatus] NVARCHAR(20) NOT NULL,
    [ScoreId] INT NOT NULL, 
    [DateOfGameOnly] NVARCHAR(50) NULL, 
    [TimeOfGameOnly] NVARCHAR(50) NULL, 
    FOREIGN KEY (HomeTeamId) REFERENCES Teams(Id),
    FOREIGN KEY (AwayTeamId) REFERENCES Teams(Id),
    FOREIGN KEY (GameWinnerId) REFERENCES Teams(Id),
)
