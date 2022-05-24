using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TonyM.Core.Events;
using TonyM.Core.Interfaces;
using TonyM.Core.Models.Opts;
using TonyM.UIDiscord.Services;

namespace TonyM.UIDiscord.Modules
{
    public class NvidiaCommand : ModuleBase
    {
        private readonly DiscordSocketClient client;
        private readonly IProductService productService;
        private readonly ISearchStatutService searchService;
        private readonly UserOptions userOptions;
        private readonly DiscordOptions discordOptions;

        public NvidiaCommand(DiscordSocketClient client, IProductService productService, ISearchStatutService searchService, IOptions<UserOptions> userOptions, IOptions<DiscordOptions> discordOptions)
        {
            this.client = client;
            this.productService = productService;
            this.searchService = searchService;
            this.discordOptions = discordOptions.Value;
            this.userOptions = userOptions.Value;
        }

        [Command("startrtx", RunMode = RunMode.Async)]
        public async Task StartSearch()
        {
            if (!this.searchService.IsSearching)
            {
                this.searchService.IsSearching = true;
                var products = productService.Initialisation(userOptions.Gpus, userOptions.Locale);

                foreach (var p in products)
                    p.OnAvailable += SendMessage;

                while (this.searchService.IsSearching)
                {
                    try
                    {
                        await productService.SearchStockAsync(products);
                        await Task.Delay(1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Erreur :" + e.Message);
                    }
                }
            }
        }

        [Command("stoprtx")]
        public async Task StopSearch()
        {
            this.searchService.IsSearching = false;
            await Context.Channel.SendMessageAsync("Fin de la recherche");
        }

        public async void SendMessage(object sender, ProductEventArgs e)
        {
            var channel = (IMessageChannel)this.client.GetChannel(this.discordOptions.DropChannel);
            var role = Context.Guild.Roles.Where(x => x.Id == this.discordOptions.DropGroup).FirstOrDefault();

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
