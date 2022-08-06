

using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using Microsoft.Extensions.Configuration;

namespace BetBookData.Services;
public class TeamService : ITeamService
{
    private readonly HttpClient _httpClient;
    private readonly ITeamData _teamData;
    private readonly IConfiguration _config;

    public TeamService(ITeamData teamData, IConfiguration config)
    {
        _httpClient = new();
        _teamData = teamData;
        _config = config;
    }

    public async Task<Team[]> GetAllTeams()
    {
        Team[] teams = new Team[34];

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

    public async Task<Team[]> GetAllTeamStats()
    {
        Team[] teams = new Team[34];

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
}
