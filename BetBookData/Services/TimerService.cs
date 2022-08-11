
using BetBookData.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

#nullable enable

public class TimerService : BackgroundService
{
    private readonly IGameService _gameService;
    private readonly PeriodicTimer _timerForPointSpreadUpdate = new(TimeSpan.FromHours(4));
    public TimerService(IGameService gameService, ILogger<TimerService> logger)
    {
        _gameService = gameService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int index = 1;
        while (await _timerForPointSpreadUpdate.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Timer trigger count: {index}");
            await _gameService.FetchAllScoresForFinishedGames();
            await _gameService.GetPointSpreadUpdateForAvailableGames();
            index++;
        }
    }
}

#nullable restore
