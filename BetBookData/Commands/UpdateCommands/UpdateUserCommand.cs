using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateUserCommand(UserModel user) : IRequest<UserModel>;

