using Spectre.Console;

namespace PersedawnyGames.TBE.ConsoleHandlers;

public class DialogWriter
{
    public static void Write(string[] dialog, int speed = 20, string? color = null)
    {
        foreach (var line in dialog)
        {
            foreach (var c in line)
            {
                AnsiConsole.Markup($"[{color}]{c}[/]");
                Thread.Sleep(speed);
            }

            AnsiConsole.Write(DialogHelpers.NewLine);
        }
    }
}