
//using BetBookData.Interfaces;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace BetBookData.Services;
//public class TimerService : BackgroundService
//{
//    private readonly IGameService _gameService;
//    private readonly PeriodicTimer _timerForPointSpreadUpdate = new(TimeSpan.FromHours(4));
//    private readonly ILogger<TimerService> _logger;
//    public TimerService(IGameService gameService, ILogger<TimerService> logger)
//    {
//        _gameService = gameService;
//        _logger = logger;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (await _timerForPointSpreadUpdate.WaitForNextTickAsync(stoppingToken)
//                    && !stoppingToken.IsCancellationRequested)
//        {
//            await _gameService.FetchAllScoresForFinishedGames();
//            await _gameService.GetPointSpreadUpdateForAvailableGames();
//        }
//    }
//}
