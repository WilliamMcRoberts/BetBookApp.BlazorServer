CREATE TABLE [dbo].[Teams]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TeamName] NVARCHAR(50) NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [Stadium] NVARCHAR(50) NOT NULL, 
    [Wins] nvarchar(1000) NOT NULL, 
    [Losses] nvarchar(1000) NOT NULL, 
    [Draws] nvarchar(1000) NOT NULL
)
