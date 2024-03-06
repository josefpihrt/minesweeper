namespace Minesweeper;

public readonly record struct CellDisplayFormat(
    char Character,
    ConsoleColor? ForegroundColor = null,
    ConsoleColor? BackgroundColor = null);
