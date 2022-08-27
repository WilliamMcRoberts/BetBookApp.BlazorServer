

using System;
using BetBookData.Helpers;
using BetBookData.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

public class ScoresUpdateTimerService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(3));
    private readonly ILogger<ScoresUpdateTimerService> _logger;

    public ScoresUpdateTimerService(IMediator mediator, ILogger<ScoresUpdateTimerService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _mediator.FetchAllScoresForFinishedGames();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Failed Scores Fetch Call...");
            }
        }
    }
}
