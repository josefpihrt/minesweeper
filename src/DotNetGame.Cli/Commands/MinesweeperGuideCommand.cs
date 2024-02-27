using System.CommandLine;
using System.Text;

namespace DotNetGame.Commands;

public class MinesweeperGuideCommand : Command
{
    public MinesweeperGuideCommand()
        : base("guide", "Guide to Minesweeper.")
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
            ("Space", "Show mines"),
#endif
        };

        var presets = new (string Name, string Description)[]
        {
            ("beginner", "9x9, 10 mines"),
            ("intermediate", "16x16, 40 mines"),
            ("expert", "30x16, 99 mines"),
        };

        int columnWidth = Math.Max(keys.Max(k => k.Key.Length), presets.Max(k => k.Name.Length)) + 2;

        this.SetHandler(
            () =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("KEYS:");

                foreach ((string Key, string Description) line in keys)
                {
                    sb.AppendLine(line.Key.PadRight(columnWidth) + line.Description);
                }

                sb.AppendLine();

                sb.AppendLine("PRESETS:");

                foreach ((string Name, string Description) line in presets)
                {
                    sb.AppendLine(line.Name.PadRight(columnWidth) + line.Description);
                }

                Console.WriteLine(sb.ToString());
            });
    }
}
