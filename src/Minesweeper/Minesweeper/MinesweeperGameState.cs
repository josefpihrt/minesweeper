using System.Diagnostics;

namespace Minesweeper;

internal sealed class MinesweeperGameState
{
    public MinesweeperGameState(MineField field, Renderer renderer)
    {
        Field = field;
        Renderer = renderer;
    }

    public Stopwatch Stopwatch { get; } = new();
    public MineField Field { get; }
    public Renderer Renderer { get; }
    public int Row => SelectedCell?.Row ?? -1;
    public int Column => SelectedCell?.Column ?? -1;
    public Cell? SelectedCell => Renderer.SelectedCell;
    public Cell? FirstSelectedCell => Renderer.FirstSelectedCell;
    public int Width => Field.Width;
    public int Height => Field.Height;

    public FieldDisplayMode FieldDisplayMode
    {
        get { return Renderer.FieldDisplayMode; }
        set { Renderer.FieldDisplayMode = value; }
    }

    public void TrySelectCell(int row, int column) => Renderer.TrySelectCell(row, column);
    public void SelectCell(Cell? cell) => Renderer.SelectCell(cell);
    public void SelectCell(int row, int column) => Renderer.SelectCell(row, column);
    public void SelectCells(int row, int column) => Renderer.SelectCells(Field[row, column]);

    public GameResult Run()
    {
        while (true)
        {
            ConsoleKeyInfo info = Console.ReadKey(intercept: true);

            bool control = info.Modifiers == ConsoleModifiers.Control;
            bool shift = info.Modifiers == ConsoleModifiers.Shift;
            bool alt = info.Modifiers == ConsoleModifiers.Alt;

            if (info is { Key: ConsoleKey.P, Modifiers: ConsoleModifiers.None })
            {
                if (FieldDisplayMode == FieldDisplayMode.HideAll)
                {
                    FieldDisplayMode = FieldDisplayMode.Default;
                    Renderer.RenderField();
                    Stopwatch.Start();
                }
                else
                {
                    Stopwatch.Stop();
                    FieldDisplayMode = FieldDisplayMode.HideAll;
                    Renderer.RenderField();
                }

                continue;
            }

            switch (info.Key)
            {
#if DEBUG
                case ConsoleKey.Spacebar:
                {
                    FieldDisplayMode = (FieldDisplayMode == FieldDisplayMode.HintMines)
                        ? FieldDisplayMode.Default
                        : FieldDisplayMode.HintMines;

                    Renderer.RenderField();
                    break;
                }
                case ConsoleKey.R:
                {
                    Renderer.Clear();
                    Renderer.RenderField();
                    break;
                }
#endif
                case ConsoleKey.A:
                {
                    if (!Stopwatch.IsRunning)
                        Stopwatch.Start();

                    Renderer.ChangeSelectedCellsState(flag: false);

                    switch (Field.State)
                    {
                        case FieldState.MineHit:
                            return GameResult.Lost;

                        case FieldState.Completed:
                            return GameResult.Won;
                    }

                    break;
                }
                case ConsoleKey.S:
                {
                    if (!Stopwatch.IsRunning)
                        Stopwatch.Start();

                    Renderer.ChangeSelectedCellsState(flag: true);

                    switch (Field.State)
                    {
                        case FieldState.MineHit:
                            return GameResult.Lost;

                        case FieldState.Completed:
                            return GameResult.Won;
                    }

                    break;
                }
                case ConsoleKey.Home:
                {
                    SelectCell((control) ? 0 : Row, 0);
                    break;
                }
                case ConsoleKey.End:
                {
                    SelectCell((control) ? Height - 1 : Row, Width - 1);
                    break;
                }
                case ConsoleKey.Escape:
                {
                    SelectCell(SelectedCell);
                    break;
                }
                case ConsoleKey.LeftArrow:
                {
                    if (shift)
                    {
                        if (Column > 0)
                            SelectCells(Row, Column - 1);
                    }
                    else if (alt)
                    {
                        Cell? cell = SelectedCell!;
                        cell = FindNearUnknownCell(cell.EnumerateCellsLeft(), cell.IsUnknown);

                        SelectCell(Row, cell?.Column ?? 0);
                    }
                    else if (Renderer.IsMultiSelection)
                    {
                        SelectCell(SelectedCell!.Row, Math.Min(FirstSelectedCell!.Column, SelectedCell.Column));
                    }
                    else
                    {
                        TrySelectCell(Row, Column - 1);
                    }

                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (shift)
                    {
                        if (Row > 0)
                            SelectCells(Row - 1, Column);
                    }
                    else if (alt)
                    {
                        Cell? cell = SelectedCell!;
                        cell = FindNearUnknownCell(cell.EnumerateCellsUp(), cell.IsUnknown);
                        SelectCell(cell?.Row ?? 0, Column);
                    }
                    else
                    {
                        TrySelectCell(Row - 1, Column);
                    }

                    break;
                }
                case ConsoleKey.RightArrow:
                {
                    if (shift)
                    {
                        if (Column < Width - 1)
                            SelectCells(Row, Column + 1);
                    }
                    else if (alt)
                    {
                        Cell? cell = SelectedCell!;
                        cell = FindNearUnknownCell(cell.EnumerateCellsRight(), cell.IsUnknown);
                        SelectCell(Row, cell?.Column ?? Width - 1);
                    }
                    else if (Renderer.IsMultiSelection)
                    {
                        SelectCell(SelectedCell!.Row, Math.Max(FirstSelectedCell!.Column, SelectedCell.Column));
                    }
                    else
                    {
                        TrySelectCell(Row, Column + 1);
                    }

                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    if (shift)
                    {
                        if (Row < Height - 1)
                            SelectCells(Row + 1, Column);
                    }
                    else if (alt)
                    {
                        Cell? cell = SelectedCell!;
                        cell = FindNearUnknownCell(cell.EnumerateCellsDown(), cell.IsUnknown);

                        SelectCell(cell?.Row ?? Height - 1, Column);
                    }
                    else
                    {
                        TrySelectCell(Row + 1, Column);
                    }

                    break;
                }
                case ConsoleKey.Q:
                {
                    return GameResult.Canceled;
                }
                case ConsoleKey.C:
                {
                    if (control)
                        return GameResult.Canceled;

                    break;
                }
            }
        }

        static Cell? FindNearUnknownCell(IEnumerable<Cell> cells, bool isUnknown)
        {
            return (isUnknown && cells.FirstOrDefault()?.IsUnknown == true)
                ? cells.TakeWhile(f => f.IsUnknown).LastOrDefault()
                : cells.SkipWhile(f => !f.IsUnknown).FirstOrDefault();
        }
    }
}
