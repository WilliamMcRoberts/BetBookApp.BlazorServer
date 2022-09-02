using BetBookData.Commands.InsertCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.InsertHandlers;

public class InsertParleyBetHandler : IRequestHandler<InsertParleyBetCommand, ParleyBetModel>
{
    private readonly IParleyBetData _parleyData;

    public InsertParleyBetHandler(IParleyBetData parleyData)
    {
        _parleyData = parleyData;
    }

    public async Task<ParleyBetModel> Handle(InsertParleyBetCommand request, CancellationToken cancellationToken)
    {
        int parleyBetId = await _parleyData.InsertParleyBet(request.parleyBet);

        request.parleyBet.Id = parleyBetId;
        
        return request.parleyBet;
    }
}
