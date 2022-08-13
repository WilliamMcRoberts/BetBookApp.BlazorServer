
using BetBookData.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BetBookData.Services;

#nullable enable

public class TimerService : BackgroundService
{
    private readonly IGameService _gameService;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));

    public TimerService(IGameService gameService)
    {
        _gameService = gameService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int index = 1;
        while (await _timer.WaitForNextTickAsync(stoppingToken)
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
