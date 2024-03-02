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

        var rootCommand = new RootCommand("Play Minesweeper on the command line.") { Name = "dotnet-minesweeper" };

        var minesweeperPlayCommand = new MinesweeperPlayCommand(appOptions.Minesweeper);
#if DEBUG
        minesweeperPlayCommand.AddAlias("p");
#endif
        rootCommand.AddCommand(minesweeperPlayCommand);
        rootCommand.AddCommand(new MinesweeperGuideCommand());

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
