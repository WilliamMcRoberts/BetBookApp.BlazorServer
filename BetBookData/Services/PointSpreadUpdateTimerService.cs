
using BetBookData.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BetBookData.Services;

#nullable enable

public class PointSpreadUpdateTimerService : BackgroundService
{
    private readonly IGameService _gameService;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));

    public PointSpreadUpdateTimerService(IGameService gameService)
    {
        _gameService = gameService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int index = 1;
        while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
        {
            await _gameService.GetPointSpreadUpdateForAvailableGames();
            Console.WriteLine($"Timer trigger count: {index} Time:{DateTime.Now}");
            index++;
        }
    }
}

#nullable restore
