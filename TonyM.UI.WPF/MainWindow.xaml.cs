using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TonyM.Core.Interfaces;
using TonyM.Core.Models.Opts;
using TonyM.UI.WPF.Helpers;

namespace TonyM.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductService productService;
        private readonly ISearchStatutService searchService;
        private readonly UserOptions userOptions;

        public MainWindow(IProductService productService, ISearchStatutService searchService, IOptions<UserOptions> userOptions)
        {
            InitializeComponent();
            this.productService = productService;
            this.searchService = searchService;
            this.userOptions = userOptions.Value;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            this.searchService.IsSearching = true;

            if (this.searchService.IsSearching)
            {
                var products = productService.Initialisation(userOptions.Gpus, userOptions.Locale);

                foreach (var p in products)
                    p.OnAvailable += UiHelpers.Alert;

                while (this.searchService.IsSearching)
                {
                    try
                    {
                        await productService.SearchStockAsync(products);
                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Nouvelle tentative...");
                    }
                }
            }
        }
    }
}
