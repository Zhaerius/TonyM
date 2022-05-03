using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TonyM.APP.Services
{
	public class LoggingService
	{
        public LoggingService(DiscordSocketClient client, CommandService commands)
		{
			client.Log += LogAsync;
            commands.Log += LogAsync;
		}

		private Task LogAsync(LogMessage message)
		{
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now} [{message.Severity}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
	}
}
