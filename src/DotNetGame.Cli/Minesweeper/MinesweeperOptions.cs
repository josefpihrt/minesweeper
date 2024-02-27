using System.Text.Json.Serialization;

namespace DotNetGame.Minesweeper;

public class MinesweeperOptions
{
    [JsonIgnore]
    public MinesweeperPreset DefaultPreset { get; set; } = null!;
    public List<MinesweeperPreset> Presets { get; set; } = [];
    public bool UseQuestionMark { get; set; }
    public bool ShowRemainingMines { get; set; }
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

    public (MineFieldOptions fieldOptions, RenderOptions renderOptions) Bind()
    {
        var fieldOptions = new MineFieldOptions()
        {
            Width = DefaultPreset.Width,
            Height = DefaultPreset.Height,
            MineCount = DefaultPreset.MineCount,
            UseQuestionMark = UseQuestionMark,
        };

        var renderOptions = new RenderOptions()
        {
            ShowRemainingMines = ShowRemainingMines,
            SelectedCellFormat = SelectedCellFormat,
            VerticalSeparatorCellFormat = VerticalSeparatorCellFormat,
            UnknownCellFormat = UnknownCellFormat,
            QuestionMarkCellFormat = QuestionMarkCellFormat,
            EmptyCellFormat = EmptyCellFormat,
            FlaggedCellFormat = FlaggedCellFormat,
            IncorrectlyFlaggedCellFormat = IncorrectlyFlaggedCellFormat,
            MineCellFormat = MineCellFormat,
            HitMineCellFormat = HitMineCellFormat,
            OneNearMineCellFormat = OneNearMineCellFormat,
            TwoNearMinesCellFormat = TwoNearMinesCellFormat,
            ThreeNearMinesCellFormat = ThreeNearMinesCellFormat,
            FourNearMinesCellFormat = FourNearMinesCellFormat,
            FiveNearMinesCellFormat = FiveNearMinesCellFormat,
            SixNearMinesCellFormat = SixNearMinesCellFormat,
            SevenNearMinesCellFormat = SevenNearMinesCellFormat,
            EightNearMinesCellFormat = EightNearMinesCellFormat,
        };

        return (fieldOptions, renderOptions);
    }
}
