using System.Diagnostics;
using TonyM.BLL.Events;

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



        public static void Alert(object sender, ProductBLEventArgs e)
        {
            OpenBuyPage(e.Link);
            SoundAlert(3);
        }

        public static void DisplayProgress(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
        }

        public static void ClearLastLine(int number)
        {
            int currentLineCursor = Console.CursorTop - number;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void TextColor(string text, ConsoleColor consoleColor)
        {
            Console.BackgroundColor = consoleColor;
            Console.Write(text);
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
