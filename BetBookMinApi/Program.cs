using BetBookData.Data;
using BetBookData.DbAccess;
using BetBookData.Interfaces;
using BetBookMinApi.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseHttpsRedirection();

app.ConfigureGamesApi();
app.ConfigureTeamsApi();
app.ConfigureUsersApi();
app.ConfigureBetsApi();
app.ConfigureParleyBetsApi();
app.ConfigureHouseAccountApi();

app.Run();

