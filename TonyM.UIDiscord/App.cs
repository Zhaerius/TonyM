using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Options;
using TonyM.Core.Models.Opts;
using TonyM.UIDiscord.Services;

namespace TonyM.UIDiscord
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