CREATE PROCEDURE [dbo].[spTeams_Update]
    @Id int,
	@TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@Wins nvarchar(1000),
	@Losses nvarchar(1000),
	@Draws nvarchar(1000)
AS
begin
    update dbo.Teams
	set TeamName = @TeamName, City = @City, Stadium = @Stadium, Wins = @Wins, Losses = @Losses, Draws = @Draws
	where Id = @Id;
end
