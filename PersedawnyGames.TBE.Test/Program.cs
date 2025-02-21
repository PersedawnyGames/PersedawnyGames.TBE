using PersedawnyGames.TBE.Test.Scenes;

namespace PersedawnyGames.TBE.Test;

internal class Program : Game
{
    static void Main(string[] args)
    {
        WindowInitializer.Initialize();
        Initialize("TBE.Test", new StartScreen());
    }
}
