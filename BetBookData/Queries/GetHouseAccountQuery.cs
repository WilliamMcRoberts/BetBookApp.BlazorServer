

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetHouseAccountQuery() : IRequest<HouseAccountModel>;

