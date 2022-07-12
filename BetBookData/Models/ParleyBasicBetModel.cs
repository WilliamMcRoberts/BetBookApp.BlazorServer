

namespace BetBookData.Models;
public class ParleyBasicBetModel
{
    public List<BasicBetModel> BasicBets { get; set; } = new();

    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }

}
