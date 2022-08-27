using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateUserAccountBalanceHandler : IRequestHandler<UpdateUserAccountBalanceCommand, UserModel>
{
    private readonly IUserData _userData;

    public UpdateUserAccountBalanceHandler(IUserData userData)
    {
        _userData = userData;
    }

    public async Task<UserModel> Handle(UpdateUserAccountBalanceCommand request, CancellationToken cancellationToken)
    {
        await _userData.UpdateUserAccountBalance(request.user);

        return request.user;
    }
}

