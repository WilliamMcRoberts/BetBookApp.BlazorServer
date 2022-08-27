using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetUserByObjectIdHandler : IRequestHandler<GetUserByObjectIdQuery, UserModel>
{
    private readonly IUserData _userData;

    public GetUserByObjectIdHandler(IUserData userData)
    {
        _userData = userData;
    }

    public async Task<UserModel> Handle(GetUserByObjectIdQuery request, CancellationToken cancellationToken)
    {
        return await _userData.GetUserFromAuthentication(request.objectId);
    }
}
