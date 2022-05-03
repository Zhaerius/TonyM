using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.APP.Services;
using TonyM.BLL.Events;
using TonyM.BLL.Services;

namespace TonyM.APP.Modules
{
    public class NvidiaCommand : ModuleBase
    {
        private readonly IBusiness _business;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly NvidiaStatutService _nvidiaStatut;

        public NvidiaCommand(DiscordSocketClient client, IServiceProvider service)
        {
            _client = client;
            _business = service.GetRequiredService<IBusiness>();
            _configuration = service.GetRequiredService<IConfiguration>();
            _nvidiaStatut = service.GetRequiredService<NvidiaStatutService>();
        }

        [Command("startrtx", RunMode = RunMode.Async)]
        public async Task StartSearch([Remainder] string args = null)
        {
            if (!_nvidiaStatut.IsRunning)
            {
                _nvidiaStatut.IsRunning = true;
                var products = _business.Initialisation();

                foreach (var p in products)
                    p.OnAvailable += SendMessage;

                while (_nvidiaStatut.IsRunning)
                {
                    var process = products.Select(async p =>
                    {
                        string? oldLink = p.BuyLink;
                        await _business.UpdateFromSourceAsync(p);
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
            ulong numberChannel = ulong.Parse(_configuration.GetSection("DropChannel").Value);
            ulong numberRole = ulong.Parse(_configuration.GetSection("DropGroup").Value);
            var channel = (IMessageChannel)_client.GetChannel(numberChannel);
            var role = Context.Guild.Roles.Where(x => x.Id == numberRole).FirstOrDefault();

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
