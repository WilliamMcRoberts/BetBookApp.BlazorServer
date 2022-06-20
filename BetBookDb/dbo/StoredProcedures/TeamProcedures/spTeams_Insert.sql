CREATE PROCEDURE [dbo].[spTeams_Insert]
    @TeamName nvarchar(50),
	@City nvarchar(50),
	@Stadium nvarchar(50),
	@WinCount int,
	@LossCount int,
	@DrawCount int,
	@Id int = 0 output
AS
begin
    insert into dbo.Teams (TeamName, City, Stadium, WinCount, LossCount, DrawCount)
	values (@TeamName, @City, @Stadium, @WinCount, @LossCount, @DrawCount);

	select @Id = SCOPE_IDENTITY();
end
