using BetBookData.Commands.InsertCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.InsertHandlers;

public class InsertUserHandler : IRequestHandler<InsertUserCommand, UserModel>
{
    private readonly IUserData _userData;

    public InsertUserHandler(IUserData userData)
    {
        _userData = userData;
    }

    public async Task<UserModel> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        await _userData.InsertUser(request.user);

        return request.user;
    }
}
