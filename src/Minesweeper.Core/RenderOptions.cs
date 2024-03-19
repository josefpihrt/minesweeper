// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Minesweeper;

public class RenderOptions
{
    public bool ShowMineCount { get; set; }
    public CellDisplayFormat SelectedCellFormat { get; set; }
    public CellDisplayFormat VerticalSeparatorCellFormat { get; set; }
    public CellDisplayFormat UnknownCellFormat { get; set; }
    public CellDisplayFormat QuestionMarkCellFormat { get; set; }
    public CellDisplayFormat EmptyCellFormat { get; set; }
    public CellDisplayFormat FlaggedCellFormat { get; set; }
    public CellDisplayFormat IncorrectlyFlaggedCellFormat { get; set; }
    public CellDisplayFormat MineCellFormat { get; set; }
    public CellDisplayFormat HitMineCellFormat { get; set; }
    public CellDisplayFormat OneNearMineCellFormat { get; set; }
    public CellDisplayFormat TwoNearMinesCellFormat { get; set; }
    public CellDisplayFormat ThreeNearMinesCellFormat { get; set; }
    public CellDisplayFormat FourNearMinesCellFormat { get; set; }
    public CellDisplayFormat FiveNearMinesCellFormat { get; set; }
    public CellDisplayFormat SixNearMinesCellFormat { get; set; }
    public CellDisplayFormat SevenNearMinesCellFormat { get; set; }
    public CellDisplayFormat EightNearMinesCellFormat { get; set; }
}
