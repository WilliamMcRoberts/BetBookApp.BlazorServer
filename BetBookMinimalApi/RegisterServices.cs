using BetBookData;
using BetBookData.Data;
using BetBookData.Interfaces;
using BetBookDbAccess;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace BetBookMinimalApi;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Jwt Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {new OpenApiSecurityScheme{Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }},  new List<string>() }
            });
        });

        builder.Services.AddMediatR(typeof(MediatREntryPoint).Assembly);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        });

        builder.Services.AddSingleton<ISqlConnection, SqlConnection>();
        builder.Services.AddTransient<IUserData, UserData>();
        builder.Services.AddTransient<ITeamData, TeamData>();
        builder.Services.AddTransient<IBetData, BetData>();
        builder.Services.AddTransient<IParleyBetData, ParleyBetData>();
        builder.Services.AddTransient<IHouseAccountData, HouseAccountData>();
        builder.Services.AddTransient<IGameData, GameData>();
    }
}
