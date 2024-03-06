using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Runtime.InteropServices;
using System.Text;
using DotNetGame.Commands;

namespace DotNetGame;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        ApplicationOptions appOptions = ApplicationOptions.Load();

        var rootCommand = new MinesweeperCommand(appOptions.Minesweeper) { Name = "minesweeper" };

        Parser parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .UseHelp(context =>
            {
                context.HelpBuilder.CustomizeLayout(
                    _ =>
                        HelpBuilder.Default
                            .GetLayout()
                            .Append(
                                _ => context.Output.WriteLine(GetAdditionalHelpText())
                    ));
            })
            .Build();

        bool treatControlCAsInput = Console.TreatControlCAsInput;
        bool? cursorVisible = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            cursorVisible = Console.CursorVisible;

        try
        {
            Console.TreatControlCAsInput = true;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.CursorVisible = false;

            return await parser.InvokeAsync(args);
        }
        finally
        {
            Console.TreatControlCAsInput = treatControlCAsInput;

            if (cursorVisible is not null)
                Console.CursorVisible = cursorVisible.Value;
        }
    }

    private static string GetAdditionalHelpText()
    {
        var keys = new (string Key, string Description)[]
        {
            ("Enter", "Open cell"),
            ("Ctrl+Enter", "Flag cell"),
            ("Ctrl+Arrow", "Jump to next unknown cell"),
            ("Shift+Arrow", "Expand selection"),
            ("P", "Pause game"),
            ("Ctrl+C", "Cancel game"),
#if DEBUG
            ("Space", "Hint mines"),
#endif
        };

        int columnWidth = keys.Max(k => k.Key.Length) + 2;

        var sb = new StringBuilder();
        sb.Append("Keybindings:");

        foreach ((string Key, string Description) in keys)
        {
            sb.AppendLine();
            sb.Append(Key.PadRight(columnWidth) + Description);
        }

        return sb.ToString();
    }
}
