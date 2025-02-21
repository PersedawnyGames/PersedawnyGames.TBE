namespace PersedawnyGames.TBE.ConsoleHandlers;

public class InputHandler
{
    public static void ClearInputBuffer()
    {
        while (System.Console.KeyAvailable)
            System.Console.ReadKey(true);
    }
}
