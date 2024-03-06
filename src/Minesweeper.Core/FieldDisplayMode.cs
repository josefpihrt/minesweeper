namespace Minesweeper;

public enum FieldDisplayMode
{
    Default,
    ShowMines,
    HideAll,
#if DEBUG
    HintMines,
#endif
}
