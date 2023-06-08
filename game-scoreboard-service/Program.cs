
using game_scoreboard_service.DataContext;
using game_scoreboard_service.Hubs;
using game_scoreboard_service.Messaging;
using game_scoreboard_service.Messaging.Interfaces;
using game_scoreboard_service.Repositories;
using game_scoreboard_service.Repositories.Interfaces;
using game_scoreboard_service.Services;
using game_scoreboard_service.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
var uriString = builder.Configuration.GetConnectionString("URI");
var primaryKeyString = builder.Configuration.GetConnectionString("PRIMARY_KEY");
var databaseName = builder.Configuration.GetConnectionString("DB_NAME");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseCosmos(uriString, primaryKeyString, databaseName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddScoped<IPlayerScoreService, PlayerScoreService>();
builder.Services.AddScoped<IMessagingSubscriber, MessagingSubscriber>();
//Register Repositories
builder.Services.AddScoped<IPlayerScoreRepository, PlayerScoreRepository>();

var jsonSerializerOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};


var app = builder.Build();
app.UseCors(builder =>
{
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<ScoreboardHub>("/scoreboard");

app.Run();
