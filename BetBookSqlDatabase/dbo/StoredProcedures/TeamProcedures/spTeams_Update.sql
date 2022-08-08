CREATE PROCEDURE [dbo].[spTeams_Update]
    @Id int,
	@TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@Wins nvarchar(1000),
	@Losses nvarchar(1000),
	@Draws nvarchar(1000),
    @Symbol nvarchar(4),
    @Division nvarchar(10),
    @Conference nvarchar(4)
AS
begin
    update dbo.Teams
	set TeamName = @TeamName, City = @City, Stadium = @Stadium, Wins = @Wins, Losses = @Losses, Draws = @Draws, Symbol = @Symbol, Division = @Division, Conference = @Conference
	where Id = @Id;
end
