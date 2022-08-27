

using BetBookData.Dto;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;


public record GetGameByScoreIdDtoQuery(int scoreId) : IRequest<GameByScoreIdDto>;
