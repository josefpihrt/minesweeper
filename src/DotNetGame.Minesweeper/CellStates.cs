namespace DotNetGame.Minesweeper;

public static class CellStates
{
    internal static string GetText(int state)
    {
        return state switch
        {
            Unknown => nameof(Unknown),
            Flagged => nameof(Flagged),
            QuestionMark => nameof(QuestionMark),
            Empty => nameof(Empty),
            _ => "Near"
        };
    }

    public const int Empty = 0;
    public const int Unknown = -1;
    public const int QuestionMark = -2;
    public const int Flagged = -3;
}
