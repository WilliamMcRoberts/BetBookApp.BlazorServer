CREATE PROCEDURE [dbo].[spTeams_Insert]
    @TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@Wins nvarchar(1000),
	@Losses nvarchar(1000),
	@Draws nvarchar(1000),
    @Symbol nvarchar(4),
	@Id int = 0 output,
    @Division nvarchar(10),
    @Conference nvarchar(4)
AS
begin
    insert into dbo.Teams (TeamName, City, Stadium, Wins, Losses, Draws, Symbol, Division)
	values (@TeamName, @City, @Stadium, @Wins, @Losses, @Draws, @Symbol, @Conference);

	select @Id = SCOPE_IDENTITY();
end
