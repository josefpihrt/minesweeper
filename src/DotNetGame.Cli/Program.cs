using System.CommandLine;
using System.CommandLine.Parsing;
using System.Runtime.InteropServices;
using DotNetGame.Commands;

namespace DotNetGame;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        ApplicationOptions appOptions = ApplicationOptions.Load();

        var rootCommand = new RootCommand("Play games on the command line.") { Name = "dotnet-game" };

        var minesweeperCommand = new Command("minesweeper", "Minesweeper.");
#if DEBUG
        minesweeperCommand.AddAlias("m");
#endif
        rootCommand.AddCommand(minesweeperCommand);

        var minesweeperPlayCommand = new MinesweeperPlayCommand(appOptions.Minesweeper);
#if DEBUG
        minesweeperPlayCommand.AddAlias("p");
#endif
        minesweeperCommand.AddCommand(minesweeperPlayCommand);
        minesweeperCommand.AddCommand(new MinesweeperGuideCommand());

        bool treatControlCAsInput = Console.TreatControlCAsInput;
        bool? cursorVisible = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            cursorVisible = Console.CursorVisible;

        try
        {
            Console.TreatControlCAsInput = true;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.CursorVisible = false;

            return await rootCommand.InvokeAsync(args);
        }
        finally
        {
            Console.TreatControlCAsInput = treatControlCAsInput;

            if (cursorVisible is not null)
                Console.CursorVisible = cursorVisible.Value;
        }
    }
}
