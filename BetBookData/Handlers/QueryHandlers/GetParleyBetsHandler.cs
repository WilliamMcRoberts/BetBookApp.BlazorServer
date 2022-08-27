using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;
public class GetParleyBetsHandler : IRequestHandler<GetParleyBetsQuery, IEnumerable<ParleyBetModel>>
{
    private readonly IParleyBetData _parleyBetData;

    public GetParleyBetsHandler(IParleyBetData parleyBetData)
    {
        _parleyBetData = parleyBetData;
    }

    public async Task<IEnumerable<ParleyBetModel>> Handle(GetParleyBetsQuery request, CancellationToken cancellationToken)
    {
        return await _parleyBetData.GetParleyBets();
    }
}
