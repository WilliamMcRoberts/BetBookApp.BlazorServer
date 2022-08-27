using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateHouseAccountHandler : IRequestHandler<UpdateHouseAccountCommand, HouseAccountModel>
{
    private readonly IHouseAccountData _houseData;

    public UpdateHouseAccountHandler(IHouseAccountData houseData)
    {
        _houseData = houseData;
    }

    public async Task<HouseAccountModel> Handle(UpdateHouseAccountCommand request, CancellationToken cancellationToken)
    {
        await _houseData.UpdateHouseAccount(request.houseAccount);

        return request.houseAccount;
    }
}
