CREATE PROCEDURE [dbo].[spParleyBets_Delete]
    @Id int
AS
begin
    delete
	from dbo.ParleyBets
	where Id = @Id;
end
