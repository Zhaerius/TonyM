using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TonyM.APP.Services;
using TonyM.BLL.Events;
using TonyM.BLL.Services;
using TonyM.Models.Opts;

namespace TonyM.APP.Modules
{
    public class NvidiaCommand : ModuleBase
    {
        private readonly IBusiness _business;
        private readonly UserOptions _userOptions;
        private readonly DiscordOptions _discordOptions;
        private readonly DiscordSocketClient _client;
        private readonly NvidiaStatutService _nvidiaStatut;

        public NvidiaCommand(DiscordSocketClient client, IServiceProvider service, IOptions<UserOptions> userOptions, IOptions<DiscordOptions> discordOptions)
        {
            _client = client;
            _discordOptions = discordOptions.Value;
            _userOptions = userOptions.Value;
            _business = service.GetRequiredService<IBusiness>();
            _nvidiaStatut = service.GetRequiredService<NvidiaStatutService>();
        }

        [Command("startrtx", RunMode = RunMode.Async)]
        public async Task StartSearch()
        {
            if (!_nvidiaStatut.IsRunning)
            {
                _nvidiaStatut.IsRunning = true;
                var products = _business.Initialisation(_userOptions.Gpus, _userOptions.Locale);

                foreach (var p in products)
                    p.OnAvailable += SendMessage;

                while (_nvidiaStatut.IsRunning)
                {
                    var process = products.Select(async p =>
                    {
                        string? oldLink = p.BuyLink;
                        await _business.UpdateProductAsync(p);
                        p.VerificationStock(oldLink);
                    });

                    try
                    {
                        await Task.WhenAll(process);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Erreur :" + e.Message);
                    }

                    if (!_nvidiaStatut.IsRunning)
                        break;

                    await Task.Delay(1000);
                }
            }
        }

        [Command("stoprtx")]
        public async Task StopSearch()
        {
            _nvidiaStatut.IsRunning = false;
            await Context.Channel.SendMessageAsync("Fin de la recherche");
        }

        public async void SendMessage(object sender, ProductBLEventArgs e)
        {
            var channel = (IMessageChannel)_client.GetChannel(_discordOptions.DropChannel);
            var role = Context.Guild.Roles.Where(x => x.Id == _discordOptions.DropGroup).FirstOrDefault();

            var message = new EmbedBuilder();
            message.Title = "Carte Graphique en stock !";
            message.Description = role.Mention;
            message.AddField($"Nvidia {e.Name}", $"{e.Link}");
            message.AddField($"Date", DateTime.Now);
            message.WithColor(Color.Green);

            await channel.SendMessageAsync(embed : message.Build());
        }

    }
}
