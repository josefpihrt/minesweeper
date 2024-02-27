namespace DotNetGame.Minesweeper;

public static class CellStates
{
    internal static string GetText(int state)
    {
        return state switch
        {
            Unknown => "Unknown",
            Flagged => "Flagged",
            Questionable => "Questionable",
            Empty => "Empty",
            _ => "Near"
        };
    }

    public const int Empty = 0;
    public const int Unknown = -1;
    public const int Questionable = -2;
    public const int Flagged = -3;
}
