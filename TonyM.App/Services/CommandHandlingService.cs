using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace TonyM.APP.Services
{
    public class CommandHandlingService
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandlingService(IServiceProvider services, DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _services = services;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _commands.CommandExecuted += CommandExecutedAsync;
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
                return;

            if (result.IsSuccess)
                return;

            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }
}
