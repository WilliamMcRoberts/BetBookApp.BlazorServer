CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(100) NOT NULL, 
    [ObjectIdentifier] NVARCHAR(100) NOT NULL, 
    [DisplayName] NVARCHAR(50) NOT NULL, 
    [AccountBalance] MONEY NULL
)
