CREATE TABLE [dbo].[Teams]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TeamName] NVARCHAR(50) NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [Stadium] NVARCHAR(50) NOT NULL, 
    [WinCount] INT NOT NULL, 
    [LossCount] INT NOT NULL, 
    [DrawCount] INT NOT NULL
)
