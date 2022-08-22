
using BetBookData.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

#nullable enable

public class PointSpreadUpdateTimerService : BackgroundService
{
    private readonly IGameService _gameService;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly ILogger<PointSpreadUpdateTimerService> _logger;


    public PointSpreadUpdateTimerService(
        IGameService gameService, ILogger<PointSpreadUpdateTimerService> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int index = 1;
        while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
        {
            await _gameService.GetPointSpreadUpdateForAvailableGames();
            _logger.LogInformation($"Point Spread Update Fetch #{index}");
            index++;
        }
    }
}

#nullable restore
