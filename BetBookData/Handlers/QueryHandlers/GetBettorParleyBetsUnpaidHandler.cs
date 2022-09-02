using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetBettorParleyBetsUnpaidHandler : IRequestHandler<GetBettorParleyBetsUnpaidQuery, IEnumerable<ParleyBetModel>>
{
    private readonly IParleyBetData _parleyData;

    public GetBettorParleyBetsUnpaidHandler(IParleyBetData parleyData)
    {
        _parleyData = parleyData;
    }

    public async Task<IEnumerable<ParleyBetModel>> Handle(
        GetBettorParleyBetsUnpaidQuery request, CancellationToken cancellationToken)
    {
        return await _parleyData.GetBettorParleyBetsUnpaid(request.userId);
    }
}