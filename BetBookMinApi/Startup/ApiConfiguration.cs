using BetBookMinApi.Api;

namespace BetBookMinApi.Startup;

public static class ApiConfiguration
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.ConfigureGamesApi();
        app.ConfigureTeamsApi();
        app.ConfigureUsersApi();
        app.ConfigureBetsApi();
        app.ConfigureParleyBetsApi();
        app.ConfigureHouseAccountApi();
    }
}
