using BetBookData;
using BetBookData.Dto;
using MediatR;


public record GetGameDtoArrayByWeekAndSeasonQuery(int week, Season season) : IRequest<GameDto[]>;
