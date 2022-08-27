using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateHouseAccountCommand(HouseAccountModel houseAccount) : IRequest<HouseAccountModel>;
