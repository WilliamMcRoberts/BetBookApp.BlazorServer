using BetBookData.Helpers;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

#nullable enable

public class ThisWeeksGamesScoresAndPointSpreadUpdateTimerService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(6));
    private readonly ILogger<ThisWeeksGamesScoresAndPointSpreadUpdateTimerService> _logger;


    public ThisWeeksGamesScoresAndPointSpreadUpdateTimerService(
        IMediator mediator, ILogger<ThisWeeksGamesScoresAndPointSpreadUpdateTimerService> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken _stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(_stoppingToken)
                    && !_stoppingToken.IsCancellationRequested)
        {
            Season season = DateTime.Now.CalculateSeason();
            int week = season.CalculateWeek(DateTime.Now);
            try
            {
                await _mediator.UpdateAll(week, season);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Failed To Update Games For Week From Timer...");
            }
        }
    }
}

#nullable restore