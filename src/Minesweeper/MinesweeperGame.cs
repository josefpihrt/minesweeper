// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Minesweeper;

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

            Console.WriteLine("Press Enter to start a new game...");

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(intercept: true);

                if (info.Key == ConsoleKey.Enter)
                    break;

                if (info.Key == ConsoleKey.Q)
                    return summary.Result;

                if (info is { Key: ConsoleKey.C, Modifiers: ConsoleModifiers.Control })
                    throw new OperationCanceledException();
            }
        }

        static void WriteElapsedTime(GameSummary summary)
        {
            Console.WriteLine($"Elapsed time: {summary.ElapsedTime.TotalMilliseconds / 1000:n1} s");
        }
    }
}
