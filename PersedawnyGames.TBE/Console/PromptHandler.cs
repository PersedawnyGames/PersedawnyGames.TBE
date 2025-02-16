using Spectre.Console;

namespace PersedawnyGames.TBE.Console;

public class PromptHandler
{
    public static string PromptQuestion(string question, string defaultValue)
    {
        InputHandler.ClearInputBuffer();
        return AnsiConsole.Ask(question, defaultValue);
    }

    public static string PromptSelection(string[] options)
    {
        InputHandler.ClearInputBuffer();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(CheckOptionsAmount(options.Length))
                .AddChoices(options));

        return choice;
    }

    public static PromptOption PromptSelection(IEnumerable<PromptOption> options, string? title)
    {
        InputHandler.ClearInputBuffer();

        var prompt = new SelectionPrompt<string>()
                .PageSize(CheckOptionsAmount(options.Count()))
                .AddChoices(options.Select(x => x.Action));

        if (string.IsNullOrEmpty(title))
            prompt.Title = title;

        var choice = AnsiConsole.Prompt(prompt);
        var chosenOption = options.First(x => x.Action == choice);

        DialogWriter.Write([chosenOption.Description], color: "blue");

        return chosenOption;
    }

    private static int CheckOptionsAmount(int choices)
    {
        if (choices < 3)
            return 3;

        return choices;
    }
}