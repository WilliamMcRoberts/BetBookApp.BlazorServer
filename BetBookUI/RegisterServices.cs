using BetBookData.DbAccess;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Syncfusion.Blazor;

namespace BetBookUI;

/// <summary>
/// Register services class
/// </summary>
public static class RegisterServices
{
    /// <summary>
    /// Method to configure project services
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Microsoft authentication
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        // Admin authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("JobTitle", "Admin");
            });
        });

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        builder.Services.AddMemoryCache();
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
        builder.Services.AddSyncfusionBlazor();

        /********************** Services *****************************/

        builder.Services.AddHostedService<PointSpreadUpdateTimerService>();
        builder.Services.AddHostedService<ScoresUpdateTimerService>();
        builder.Services.AddSingleton<IGameService, GameService>();

        /***************** Http Client Factory **********************/

        builder.Services.AddHttpClient("sportsdata", client =>
        {
            client.BaseAddress = new Uri("https://api.sportsdata.io/v3/nfl/");
        });

        /*********************** Data access *************************/

        builder.Services.AddSingleton<ISqlConnection, SqlConnection>();
        builder.Services.AddTransient<IUserData, UserData>();
        builder.Services.AddTransient<ITeamData, TeamData>();
        builder.Services.AddTransient<IBetData, BetData>();
        builder.Services.AddTransient<IGameData, GameData>();
        builder.Services.AddTransient<IHouseAccountData, HouseAccountData>();
        builder.Services.AddTransient<IParleyBetData, ParleyBetData>();
    }
}
