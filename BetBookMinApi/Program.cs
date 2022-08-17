using BetBookData.Data;
using BetBookData.DbAccess;
using BetBookData.Interfaces;
using BetBookMinApi.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});


builder.Services.AddSingleton<ISqlConnection, SqlConnection>();
builder.Services.AddTransient<IGameData, GameData>();
builder.Services.AddTransient<IUserData, UserData>();
builder.Services.AddTransient<ITeamData, TeamData>();
builder.Services.AddTransient<IBetData, BetData>();
builder.Services.AddTransient<IParleyBetData, ParleyBetData>();
builder.Services.AddTransient<IHouseAccountData, HouseAccountData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.ConfigureGamesApi();
app.ConfigureTeamsApi();
app.ConfigureUsersApi();
app.ConfigureBetsApi();
app.ConfigureParleyBetsApi();
app.ConfigureHouseAccountApi();

app.Run();

