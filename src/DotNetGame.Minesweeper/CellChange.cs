namespace DotNetGame.Minesweeper;

public readonly record struct CellChange(Cell Cell, int State, int NewState);
