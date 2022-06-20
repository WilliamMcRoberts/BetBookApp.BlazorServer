CREATE PROCEDURE [dbo].[spBets_Delete]
    @Id int
AS
begin
    delete
	from dbo.Bets
	where Id = @Id;
end
