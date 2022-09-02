using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetBettorBetsUnpaidHandler : IRequestHandler<GetBettorBetsUnpaidQuery, IEnumerable<BetModel>>
{
    private readonly IBetData _betData;

    public GetBettorBetsUnpaidHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<IEnumerable<BetModel>> Handle(GetBettorBetsUnpaidQuery request, CancellationToken cancellationToken)
    {
        return await _betData.GetBettorBetsUnpaid(request.userId);
    }
}