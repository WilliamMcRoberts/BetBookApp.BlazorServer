using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetBetsHandler : IRequestHandler<GetBetsQuery, IEnumerable<BetModel>>
{
    private readonly IBetData _betData;

    public GetBetsHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<IEnumerable<BetModel>> Handle(GetBetsQuery request, CancellationToken cancellationToken)
    {
        return await _betData.GetBets();
    }
}
