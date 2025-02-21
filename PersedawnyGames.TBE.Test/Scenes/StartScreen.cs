using PersedawnyGames.TBE.ConsoleHandlers;

namespace PersedawnyGames.TBE.Test.Scenes;

internal class StartScreen : IScene
{
    public void Execute()
    {
        var options = new List<PromptOption> {
            new PromptOption
            {
                Action = "Yes",
                Description = "Great! I hope TBE can help you!"
            },
            new PromptOption
            {
                Action = "No",
                Description = "That's to bad..."
            }
        };

        PromptHandler.PromptSelection(options, "Do you like making games?");
    }
}
