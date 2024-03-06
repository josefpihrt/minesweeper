namespace DotNetGame.Minesweeper;

public class MinesweeperGame
{
    public MinesweeperGame(MineFieldOptions fieldOptions, RenderOptions renderOptions)
    {
        FieldOptions = fieldOptions;
        RenderOptions = renderOptions;
    }

    public MineFieldOptions FieldOptions { get; }

    public RenderOptions RenderOptions { get; }

    public GameResult RunLoop()
    {
        while (true)
        {
            var field = new MineField(new ChangeTracker(), FieldOptions);

            var renderer = new Renderer(field, RenderOptions);

            renderer.RenderField();

            var state = new MinesweeperGameState(field, renderer);

            GameResult result = state.Run();

            var summary = new GameSummary(result, state.Stopwatch.Elapsed);

            switch (summary.Result)
            {
                case GameResult.Won:
                    ConsoleUtility.WriteLine("Congratulations, you won!", ConsoleColor.Green);
                    WriteElapsedTime(summary);
                    break;

                case GameResult.Lost:
                    ConsoleUtility.WriteLine("You hit a mine!", ConsoleColor.Red);
                    WriteElapsedTime(summary);
                    break;

                case GameResult.Canceled:
                    ConsoleUtility.WriteLine("Game canceled.");
                    break;

                default:
                    throw new InvalidOperationException();
            }

            Console.WriteLine("Start another game (y/n)?");

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(intercept: true);

                if (info.Key == ConsoleKey.Y)
                {
                    break;
                }
                else if (info.Key == ConsoleKey.N)
                {
                    return summary.Result;
                }
                else if (info.Key == ConsoleKey.C)
                {
                    if (info.Modifiers == ConsoleModifiers.Control)
                        return GameResult.Canceled;
                }
            }
        }

        static void WriteElapsedTime(GameSummary summary)
        {
            Console.WriteLine($"Elapsed time: {summary.ElapsedTime.TotalMilliseconds / 1000:n1} s");
        }
    }
}
