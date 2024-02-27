using System.Diagnostics;

namespace DotNetGame.Minesweeper;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class Cell
{
    public Cell(int row, int column, MineField field)
    {
        Row = row;
        Column = column;
        Field = field;
    }

    public bool IsUnknown => State == CellStates.Unknown || State == CellStates.QuestionMark;

    public bool IsQuestionMark => State == CellStates.QuestionMark;

    public bool IsFlagged => State == CellStates.Flagged;

    public bool IsEmpty => State == CellStates.Empty;

    public int Row { get; }

    public int Column { get; }

    public MineField Field { get; }

    public bool ContainsMine { get; internal set; }

    public int State { get; internal set; } = CellStates.Unknown;

    public int NearMinesCount => Math.Max(State, -1);

    public IEnumerable<Cell> GetAdjacentCells()
    {
        int i = Row;
        int j = Column;

        if (i > 0)
        {
            if (j > 0)
                yield return Field[i - 1, j - 1];

            yield return Field[i - 1, j];

            if (j < Field.Width - 1)
                yield return Field[i - 1, j + 1];
        }

        if (j < Field.Width - 1)
        {
            yield return Field[i, j + 1];

            if (i < Field.Height - 1)
                yield return Field[i + 1, j + 1];
        }

        if (i < Field.Height - 1)
        {
            yield return Field[i + 1, j];

            if (j > 0)
                yield return Field[i + 1, j - 1];
        }

        if (j > 0)
            yield return Field[i, j - 1];
    }

    public IEnumerable<Cell> EnumerateCellsLeft()
    {
        for (int i = Column - 1; i >= 0; i--)
            yield return Field[Row, i];
    }

    public IEnumerable<Cell> EnumerateCellsRight()
    {
        for (int i = Column + 1; i < Field.Width; i++)
            yield return Field[Row, i];
    }

    public IEnumerable<Cell> EnumerateCellsUp()
    {
        for (int i = Row - 1; i >= 0; i--)
            yield return Field[i, Column];
    }

    public IEnumerable<Cell> EnumerateCellsDown()
    {
        for (int i = Row + 1; i < Field.Height; i++)
            yield return Field[i, Column];
    }

    private string GetDebuggerDisplay()
    {
        string state = CellStates.GetText(State);

        if (NearMinesCount > 0)
            state += $" {NearMinesCount}";

        return $"[{Row}, {Column}] {state} ContainsMine = {ContainsMine}";
    }
}
