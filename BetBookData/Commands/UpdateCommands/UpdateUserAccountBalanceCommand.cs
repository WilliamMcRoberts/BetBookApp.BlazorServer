using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateUserAccountBalanceCommand(UserModel user) : IRequest<UserModel>;

