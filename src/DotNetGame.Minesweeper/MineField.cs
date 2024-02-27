using System.Diagnostics;

namespace DotNetGame.Minesweeper;

public sealed class MineField
{
    private readonly Cell[,] _cells;
    private readonly HashSet<Cell> _unknownCells = [];
    private readonly ChangeTracker? _changeTracker;

    public MineField(ChangeTracker? changeTracker = null, MineFieldOptions? options = null)
    {
        _changeTracker = changeTracker;
        Options = options ?? new MineFieldOptions();
        MineCount = Options.MineCount;

        _cells = new Cell[Height, Width];
        var cells = new List<Cell>(CellCount);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var cell = new Cell(i, j, this);
                _cells[i, j] = cell;
                cells.Add(cell);
                _unknownCells.Add(cell);
            }
        }

        int mineCount = MineCount;
        var random = new Random();

        while (mineCount > 0)
        {
            int i = random.Next(0, cells.Count);

            cells[i].ContainsMine = true;
            cells.RemoveAt(i);
            mineCount--;
        }
    }

    public IReadOnlyList<CellChange>? Changes => _changeTracker?.Changes;

    public MineFieldOptions Options { get; }

    public int Width => Options.Width;

    public int Height => Options.Height;

    public int CellCount => Width * Height;

    public int MineCount { get; }

    public int FlagCount { get; private set; }

    internal Cell? HitCell { get; private set; }

    public FieldState State { get; private set; }

    public Cell this[int row, int column]
    {
        get
        {
            if (row >= Height
                || column >= Width)
            {
                throw new IndexOutOfRangeException();
            }

            return _cells[row, column];
        }
    }

    internal void ChangeCellsState(IEnumerable<Cell> cells, bool flag)
    {
        Debug.Assert(State == FieldState.None);

        var newlyEmptyCells = new List<Cell>();

        foreach (Cell cell in cells)
        {
            if (flag)
            {
                if (cell.IsFlagged)
                {
                    UpdateCellState(cell, (Options.UseQuestionMark) ? CellStates.QuestionMark : CellStates.Unknown);
                }
                else if (cell.IsQuestionMark)
                {
                    UpdateCellState(cell, CellStates.Unknown);
                }
                else if (cell.IsUnknown)
                {
                    UpdateCellState(cell, CellStates.Flagged);
                }
            }
            else if (cell.IsUnknown)
            {
                if (cell.ContainsMine)
                {
                    HitCell = cell;
                    State = FieldState.MineHit;
                    return;
                }

                UpdateCellState(cell, CellStates.Empty);
                newlyEmptyCells.Add(cell);
            }

            if (State == FieldState.Completed)
                return;
        }

        if (newlyEmptyCells.Any())
            DetermineNearCells(newlyEmptyCells);

        if (State == FieldState.Completed)
            return;

        foreach (Cell cell in _unknownCells)
        {
            if (IsObviousMine(cell))
            {
                UpdateCellState(cell, CellStates.Flagged);

                if (State == FieldState.Completed)
                    return;
            }
        }

        static bool IsObviousMine(Cell cell)
        {
            int sum = 0;
            foreach (Cell adjacentCell in cell.GetAdjacentCells())
            {
                if (adjacentCell.NearMinesCount > 0)
                {
                    sum += adjacentCell.NearMinesCount;
                }
                else if (adjacentCell.NearMinesCount < 0)
                {
                    return false;
                }
            }

            Debug.Assert(sum > 0);

            return sum > 0;
        }
    }

    private void DetermineNearCells(IEnumerable<Cell> cells)
    {
#if DEBUG
        var included = new HashSet<Cell>();
#endif
        var queue = new Queue<Cell>(cells);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();

            foreach (Cell nearCell in cell.GetAdjacentCells())
            {
                if (nearCell.IsUnknown
                    && !nearCell.ContainsMine)
                {
                    int count = nearCell.GetAdjacentCells().Count(c => c.ContainsMine);

                    if (count > 0)
                    {
                        UpdateCellState(nearCell, count);

                        if (State == FieldState.Completed)
                            return;
                    }
                    else
                    {
                        UpdateCellState(nearCell, CellStates.Empty);

                        if (State == FieldState.Completed)
                            return;
#if DEBUG
                        Debug.Assert(!included.Contains(nearCell));
#endif
                        queue.Enqueue(nearCell);
                    }
                }
            }
        }
    }

    private void UpdateCellState(Cell cell, int state)
    {
        Debug.Assert(cell.State != state);

        _changeTracker?.Changes.Add(new CellChange(cell, cell.State, state));

        switch (state)
        {
            case CellStates.Flagged:
                Debug.Assert(cell.State == CellStates.Unknown);
                _unknownCells.Remove(cell);
                FlagCount++;
                break;

            case CellStates.QuestionMark:
                Debug.Assert(cell.State == CellStates.Flagged);
                _unknownCells.Add(cell);
                FlagCount--;
                break;

            case CellStates.Unknown:
                if (cell.State == CellStates.Flagged)
                {
                    _unknownCells.Add(cell);
                    FlagCount--;
                }
                else
                {
                    Debug.Assert(cell.State == CellStates.QuestionMark);
                }

                break;

            case CellStates.Empty:
                Debug.Assert(cell.State == CellStates.Unknown);
                _unknownCells.Remove(cell);
                break;

            default:
                Debug.Assert(cell.State == CellStates.Unknown && state > 0);
                _unknownCells.Remove(cell);
                break;
        }

        cell.State = state;

        if ((_unknownCells.Count == 0 && FlagCount == MineCount)
            || MineCount - FlagCount == _unknownCells.Count)
        {
            State = FieldState.Completed;
        }
    }
}
