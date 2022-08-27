
using BetBookData.Helpers;
using BetBookData.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

#nullable enable

public class PointSpreadUpdateTimerService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(3));
    private readonly ILogger<PointSpreadUpdateTimerService> _logger;


    public PointSpreadUpdateTimerService(
        IMediator mediator, ILogger<PointSpreadUpdateTimerService> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _mediator.GetPointSpreadUpdateForAvailableGames();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Failed Point Spread Update Call...");
            }
        }
    }
}

#nullable restore
