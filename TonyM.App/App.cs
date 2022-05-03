using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using TonyM.APP.Services;

namespace TonyM.APP
{
    public class App
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandlingService _commandHandling;
        private readonly IConfiguration _config;

        public App(DiscordSocketClient client, IConfiguration configuration, CommandHandlingService commandHandling, LoggingService logging)
        {
            _client = client;
            _commandHandling = commandHandling;
            _config = configuration;
        }

        public async Task Run()
        {
            await _client.LoginAsync(TokenType.Bot, _config.GetSection("Token").Value);
            await _client.StartAsync();

            await _commandHandling.InstallCommandsAsync();

            await Task.Delay(-1);
        }
    }
}
