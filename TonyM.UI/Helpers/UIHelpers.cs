using System.Diagnostics;
using TonyM.Core.Events;

namespace TonyM.APP.Helpers
{
    public static class UiHelpers
    {
        public static string Logo = @"
  _____                 __  __         ____  
 |_   _|__  _ __  _   _|  \/  | __   _| ___| 
   | |/ _ \| '_ \| | | | |\/| | \ \ / /___ \ 
   | | (_) | | | | |_| | |  | |  \ V / ___) |
   |_|\___/|_| |_|\__, |_|  |_|   \_/ |____/ 
                  |___/                      
";



        public static void Alert(object sender, ProductEventArgs e)
        {
            OpenBuyPage(e.Link);
            SoundAlert(3);
            Console.WriteLine($"{e.Name} disponible ({DateTime.Now})");
        }

        public static void TextColor(string text, ConsoleColor consoleColor)
        {
            Console.BackgroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void OpenBuyPage(string url)
        {
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = url
            };
            Process.Start(psi);
        }

        private static void SoundAlert(int nbAlert)
        {
            for (int i = 0; i < nbAlert; i++)
                Console.Beep();
        }
    }
}
