using Discord.WebSocket;
using Discord;
using TonyM.APP.Services;
using TonyM.Models.Opts;
using Microsoft.Extensions.Options;

namespace TonyM.APP
{
    public class App
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandlingService _commandHandling;
        private readonly DiscordOptions _discordOptions;

        public App(DiscordSocketClient client, CommandHandlingService commandHandling, LoggingService logging, IOptions<DiscordOptions> discordOptions)
        {
            _client = client;
            _commandHandling = commandHandling;
            _discordOptions = discordOptions.Value;
        }

        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _discordOptions.Token);
            await _client.StartAsync();

            await _commandHandling.InstallCommandsAsync();

            await Task.Delay(-1);
        }
    }
}
