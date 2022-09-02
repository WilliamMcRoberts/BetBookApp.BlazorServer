using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetInProgressParleyBetsHandler : IRequestHandler<GetInProgressParleyBetsQuery, IEnumerable<ParleyBetModel>>
{
    private readonly IParleyBetData _parleyData;

    public GetInProgressParleyBetsHandler(IParleyBetData parleyData)
    {
        _parleyData = parleyData;
    }

    public async Task<IEnumerable<ParleyBetModel>> Handle(GetInProgressParleyBetsQuery request, CancellationToken cancellationToken)
    {
        return await _parleyData.GetInProgressParleyBets();
    }
}