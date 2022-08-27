

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetUserByObjectIdQuery(string objectId) : IRequest<UserModel>;

