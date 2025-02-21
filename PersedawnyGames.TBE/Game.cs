namespace PersedawnyGames.TBE;

public class Game
{
    public static GameState GameState { get; set; }
    public static IScene Scene { get; set; }
    public string Title { get; set; }

    public static void Initialize(string title, IScene scene)
    {
        GameState = GameState.StartScreen;
        Scene = scene;

        scene.Execute();
    }
}