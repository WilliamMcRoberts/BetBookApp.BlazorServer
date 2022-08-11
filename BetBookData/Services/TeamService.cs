

using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using Microsoft.Extensions.Configuration;

namespace BetBookData.Services;

#nullable enable

public class TeamService : ITeamService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public TeamService(IConfiguration config)
    {
        _httpClient = new();
        _config = config;
    }

    public async Task<Team[]> GetAllTeams()
    {
        Team[] teams = new Team[32];

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/scores/json/Teams?key={_config.GetSection("SportsDataIO").GetSection("Key2").Value}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                teams = JsonSerializer.Deserialize<Team[]>(data)!;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return teams;
    }

    public async Task<TeamGameStat[]> GetAllTeamGameStatsBySeasonAndWeek(
            string season, int week)
    {
        TeamGameStat[] teamGameStats = new TeamGameStat[32];

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/scores/json/TeamGameStats/{season}/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key2").Value}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                teamGameStats = JsonSerializer.Deserialize<TeamGameStat[]>(data)!;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return teamGameStats;
    }
}

#nullable restore
