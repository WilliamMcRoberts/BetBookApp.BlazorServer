using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserModel>
{
    private readonly IUserData _userData;

    public UpdateUserHandler(IUserData userData)
    {
        _userData = userData;
    }

    public async Task<UserModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _userData.UpdateUser(request.user);

        return request.user;
    }
}
