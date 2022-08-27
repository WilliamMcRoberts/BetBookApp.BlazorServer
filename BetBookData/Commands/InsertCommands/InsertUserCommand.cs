using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.InsertCommands;

public record InsertUserCommand(UserModel user) : IRequest<UserModel>;

