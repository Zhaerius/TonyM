using System.Diagnostics;
using TonyM.BLL.Events;

namespace TonyM.UI
{
    public static class UiHelpers
    {
        public static void Alert(object sender, ProductBLEventArgs e)
        {
            OpenBuyPage(e.link);
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

        public static void ErrorTextColor(string text)
        {
            Console.BackgroundColor = ConsoleColor.Red;
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
