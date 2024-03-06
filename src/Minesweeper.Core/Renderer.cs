using System.Text;

namespace Minesweeper;

public class Renderer
{
    private readonly int _left;
    private readonly int _top;
    private readonly int _fieldLeft;
    private readonly int _fieldTop;
    private Cell? _hitCell;

    public Renderer(MineField field, RenderOptions? options = null)
    {
        Field = field;
        Options = options ?? new RenderOptions();

        for (int i = 0; i < Console.WindowHeight - 1; i++)
            Console.WriteLine();

        _left = 0;
        _fieldLeft = _left;
        _top = 0;
        _fieldTop = _top;

        Width = Field.Width;

        if (ShowSeparator)
            Width *= 2;

        Height = Field.Height + 1;

        if (Options.ShowMineCount)
        {
            Height++;
            _fieldTop++;
        }

        SelectedCell = Field[0, 0];
    }

    public MineField Field { get; }

    public RenderOptions Options { get; }

    public Cell? SelectedCell { get; private set; }

    public Cell? FirstSelectedCell { get; private set; }

    public bool IsMultiSelection => FirstSelectedCell is not null;

    public FieldDisplayMode FieldDisplayMode { get; set; }

    public int Height { get; }

    public int Width { get; }

    private bool ShowSeparator => Options.VerticalSeparatorCellFormat.Character != default;

    private IReadOnlyList<CellChange>? CellChanges => Field.Changes;

    public void ChangeSelectedCellsState(bool flag)
    {
        int changeCount = CellChanges?.Count ?? 0;

        Field.ChangeCellsState(SelectedCells(), flag);

        FieldState state = Field.State;

        if (state == FieldState.MineHit
            || state == FieldState.Completed)
        {
            SelectedCell = null;
            FirstSelectedCell = null;

            _hitCell = Field.HitCell;

            FieldDisplayMode = FieldDisplayMode.ShowMines;

            RenderField();
        }
        else if (CellChanges?.Count > changeCount)
        {
            if (Options.ShowMineCount)
            {
                SetCursorPosition(_left, _top);
                Console.Write(new string(' ', Width));
                SetCursorPosition(_left, _top);
                Console.WriteLine($"Mines: {Field.MineCount - Field.FlagCount}");
            }

            for (int i = changeCount; i < CellChanges.Count; i++)
                RenderCell(CellChanges[i].Cell);

            foreach (Cell cell in SelectedCells())
                RenderSelectedCell(cell);

            SetCursorPosition(_left, _top + Height - 1);
        }
        else
        {
            RenderField();
        }
    }

    public void RenderField()
    {
        SetCursorPosition(_left, _top);

        if (FieldDisplayMode == FieldDisplayMode.HideAll)
        {
            Clear();
            SetCursorPosition(_fieldLeft + (Field.Width / 2) + 3, _fieldTop + (Field.Height / 2) - 1);
            Console.Write("PAUSED");
            SetCursorPosition(_left, _top + Height - 1);
        }
        else
        {
            if (Options.ShowMineCount)
            {
                Console.Write(new string(' ', Width));
                SetCursorPosition(_left, _top);
                Console.WriteLine($"Mines: {Field.MineCount - Field.FlagCount}");
            }

            for (int i = 0; i < Field.Height; i++)
            {
                for (int j = 0; j < Field.Width; j++)
                {
                    WriteSeparator();

                    bool isSelected = IsSelectedCell(i, j);

                    RenderCell(
                        Field[i, j],
                        (isSelected) ? Options.SelectedCellFormat.ForegroundColor : null,
                        (isSelected) ? Options.SelectedCellFormat.BackgroundColor : null);
                }

                WriteSeparator();
                Console.WriteLine();
            }
        }

        void WriteSeparator() => ConsoleUtility.Write(Options.VerticalSeparatorCellFormat);
    }

    private void RenderCell(
        Cell cell,
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null)
    {
        int left = _fieldLeft + (cell.Column * ((ShowSeparator) ? 2 : 1));

        SetCursorPosition(left, _fieldTop + cell.Row);

        if (FieldDisplayMode == FieldDisplayMode.HideAll)
        {
            ConsoleUtility.Write(" ");
        }
        else
        {
            CellDisplayFormat cellFormat = GetCellText(cell);
#if DEBUG
            if (FieldDisplayMode == FieldDisplayMode.HintMines
                && cell.ContainsMine
                && (cell.IsUnknown || cell.IsFlagged))
            {
                cellFormat = cellFormat with { BackgroundColor = ConsoleColor.DarkGray };
            }
#endif
            ConsoleUtility.Write(
                cellFormat.Character,
                foregroundColor ?? cellFormat.ForegroundColor,
                backgroundColor ?? cellFormat.BackgroundColor);
        }
    }

    private CellDisplayFormat GetCellText(Cell cell)
    {
        if (cell.IsUnknown)
        {
            if (FieldDisplayMode == FieldDisplayMode.ShowMines)
            {
                if (cell.ContainsMine)
                {
                    return (cell == _hitCell) ? Options.HitMineCellFormat : Options.MineCellFormat;
                }

                return Options.EmptyCellFormat;
            }

            return (cell.IsQuestionMark) ? Options.QuestionMarkCellFormat : Options.UnknownCellFormat;
        }
        else if (cell.IsEmpty)
        {
            return Options.EmptyCellFormat;
        }
        else if (cell.IsFlagged)
        {
            if (!cell.ContainsMine
                && FieldDisplayMode == FieldDisplayMode.ShowMines)
            {
                return Options.IncorrectlyFlaggedCellFormat;
            }

            return Options.FlaggedCellFormat;
        }
        else if (cell.NearMinesCount > 0)
        {
            return cell.NearMinesCount switch
            {
                1 => Options.OneNearMineCellFormat,
                2 => Options.TwoNearMinesCellFormat,
                3 => Options.ThreeNearMinesCellFormat,
                4 => Options.FourNearMinesCellFormat,
                5 => Options.FiveNearMinesCellFormat,
                6 => Options.SixNearMinesCellFormat,
                7 => Options.SevenNearMinesCellFormat,
                8 => Options.EightNearMinesCellFormat,
                _ => throw new InvalidOperationException(),
            };
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private void RenderSelectedCell(Cell cell)
    {
        RenderCell(cell, foregroundColor: Options.SelectedCellFormat.ForegroundColor, backgroundColor: Options.SelectedCellFormat.BackgroundColor);
    }

    public void TrySelectCell(int row, int column)
    {
        if (row >= 0
            && row < Field.Height
            && column >= 0
            && column < Field.Width)
        {
            SelectCell(row, column);
        }
    }

    public void SelectCell(int row, int column)
    {
        SelectCell(Field[row, column]);
    }

    public void SelectCell(Cell? cell)
    {
        UnselectCells();

        if (cell is not null)
            RenderSelectedCell(cell);

        SetCursorPosition(_left, _top + Height - 1);

        FirstSelectedCell = null;
        SelectedCell = cell;
    }

    public void SelectCells(Cell cell)
    {
        Cell? cell2 = FirstSelectedCell ?? SelectedCell;

        UnselectCells();

        if (cell2 is not null
            && cell != cell2)
        {
            foreach (Cell cell3 in GetCellsInRectangle(cell, cell2))
                RenderSelectedCell(cell3);

            SetCursorPosition(_left, _top + Height - 1);

            FirstSelectedCell = cell2;
            SelectedCell = cell;
        }
        else
        {
            SelectCell(cell);
        }

        SelectedCell = cell;
    }

    private void UnselectCells()
    {
        if (FirstSelectedCell is not null)
        {
            foreach (Cell cell in SelectedCells())
                RenderCell(cell);
        }
        else if (SelectedCell is not null)
        {
            RenderCell(SelectedCell);
        }
    }

    private bool IsSelectedCell(int row, int column)
    {
        if (SelectedCell is not null)
        {
            if (FirstSelectedCell is not null)
            {
                return row >= Math.Min(FirstSelectedCell!.Row, SelectedCell.Row)
                    && row <= Math.Max(FirstSelectedCell!.Row, SelectedCell.Row)
                    && column >= Math.Min(FirstSelectedCell!.Column, SelectedCell.Column)
                    && column <= Math.Max(FirstSelectedCell!.Column, SelectedCell.Column);
            }
            else
            {
                return row == SelectedCell.Row
                    && column == SelectedCell.Column;
            }
        }

        return false;
    }

    public IEnumerable<Cell> SelectedCells()
    {
        if (SelectedCell is not null)
        {
            if (FirstSelectedCell is not null)
            {
                foreach (Cell cell in GetCellsInRectangle(FirstSelectedCell, SelectedCell))
                    yield return cell;
            }
            else
            {
                yield return SelectedCell;
            }
        }
    }

    private IEnumerable<Cell> GetCellsInRectangle(Cell first, Cell last)
    {
        int rowStart = Math.Min(first!.Row, last!.Row);
        int rowEnd = Math.Max(first!.Row, last!.Row);

        int columnStart = Math.Min(first!.Column, last!.Column);
        int columnEnd = Math.Max(first!.Column, last!.Column);

        for (int i = rowStart; i <= rowEnd; i++)
        {
            for (int j = columnStart; j <= columnEnd; j++)
                yield return Field[i, j];
        }
    }

    private void Clear()
    {
        var line = new string(' ', Width);
        var sb = new StringBuilder();

        for (int i = 0; i < Height - 1; i++)
            sb.AppendLine(line);

        Console.Write(sb.ToString());
    }

    private static void SetCursorPosition(int left, int top)
    {
        Console.SetCursorPosition(left, top);
    }
}
