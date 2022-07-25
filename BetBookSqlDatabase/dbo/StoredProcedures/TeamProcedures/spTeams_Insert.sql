CREATE PROCEDURE [dbo].[spTeams_Insert]
    @TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@Wins nvarchar(1000),
	@Losses nvarchar(1000),
	@Draws nvarchar(1000),
	@Id int = 0 output
AS
begin
    insert into dbo.Teams (TeamName, City, Stadium, Wins, Losses, Draws)
	values (@TeamName, @City, @Stadium, @Wins, @Losses, @Draws);

	select @Id = SCOPE_IDENTITY();
end
