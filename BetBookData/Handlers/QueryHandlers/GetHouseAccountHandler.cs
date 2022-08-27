using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetHouseAccountHandler : IRequestHandler<GetHouseAccountQuery, HouseAccountModel>
{
    private readonly IHouseAccountData _houseData;

    public GetHouseAccountHandler(IHouseAccountData houseData)
    {
        _houseData = houseData;
    }

    public Task<HouseAccountModel> Handle(GetHouseAccountQuery request, CancellationToken cancellationToken)
    {
        return _houseData.GetHouseAccount();
    }
}
