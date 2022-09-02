using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetBetsOnCurrentGameHandler : IRequestHandler<GetBetsOnCurrentGameQuery, IEnumerable<BetModel>>
{
    private readonly IBetData _betData;

    public GetBetsOnCurrentGameHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<IEnumerable<BetModel>> Handle(GetBetsOnCurrentGameQuery request, CancellationToken cancellationToken)
    {
        return await _betData.GetBetsOnCurrentGame(request.gameId);
    }
}