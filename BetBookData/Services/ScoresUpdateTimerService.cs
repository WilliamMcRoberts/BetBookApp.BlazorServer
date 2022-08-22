

using BetBookData.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

public class ScoresUpdateTimerService : BackgroundService
{
    private readonly IGameService _gameService;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly ILogger<ScoresUpdateTimerService> _logger;

    public ScoresUpdateTimerService(IGameService gameService, ILogger<ScoresUpdateTimerService> logger)
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
            await _gameService.FetchAllScoresForFinishedGames();
            _logger.LogInformation($"Scores Fetch #{index}");
            index++;
        }
    }
}
