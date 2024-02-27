using System.CommandLine;
using DotNetGame.Minesweeper;

namespace DotNetGame.Commands;

public class MinesweeperPlayCommand : Command
{
    public MinesweeperPlayCommand(MinesweeperOptions minesweeperOptions)
        : base("play", "Play Minesweeper.")
    {
        MinesweeperOptions = minesweeperOptions;

        PresetArgument = new Argument<string?>("PRESET", "A size and a number of mines. Predefined values are '[b]eginner', '[i]ntermediate', '[e]xpert' and 'max'.")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        WidthOption = new Option<int?>("--width", "Mine field width.");
        HeightOption = new Option<int?>("--height", "Mine field height.");
        MineCountOption = new Option<int?>("--mines", "A number of mines.");

        DensityOption = new Option<int?>("--density", "Mines density in percents.");

        DensityOption.AddValidator(result =>
        {
            int? density = result.GetValueOrDefault<int?>();

            if (density <= 0
                || density >= 100)
            {
                result.ErrorMessage = "Density must be between 1 and 99.";
            }
        });

        QuestionableOption = new Option<bool?>("--questionable", "Allow marking cells as questionable.");
        NoSeparatorOption = new Option<bool?>("--no-separator", "Do not render separator between each cell in a row.");
        NoRemainingMines = new Option<bool?>("--no-remaining-mines", "Do not show remaining unflagged mines.");

        AddArgument(PresetArgument);
        AddOption(WidthOption);
        AddOption(HeightOption);
        AddOption(MineCountOption);
        AddOption(DensityOption);
        AddOption(QuestionableOption);
        AddOption(NoSeparatorOption);
        AddOption(NoRemainingMines);

        AddValidator(result =>
        {
            MinesweeperOptions.DefaultPreset = MinesweeperOptions.Presets.First(f => f.IsDefault);
            MinesweeperPreset preset = MinesweeperOptions.DefaultPreset;

            string? presetName = result.GetValueForArgument(PresetArgument);

            if (presetName is not null)
            {
                char presetShortName = presetName[0];

                MinesweeperPreset? preset2 = MinesweeperOptions.Presets.Find(f => f.Name == presetName || f.ShortName == presetShortName);

                if (preset2 is null)
                {
                    result.ErrorMessage = $"Preset '{presetName}' was not found.";
                    return;
                }

                preset = preset2;
            }

            int? widthOpt = result.GetValueForOption(WidthOption);
            int? heightOpt = result.GetValueForOption(HeightOption);
            int? mineCountOpt = result.GetValueForOption(MineCountOption);
            int? densityOpt = result.GetValueForOption(DensityOption);
            bool? useQuestionableOpt = result.GetValueForOption(QuestionableOption);
            bool? noVerticalSeparatorOpt = result.GetValueForOption(NoSeparatorOption);
            bool? noRemainingMinesOpt = result.GetValueForOption(NoRemainingMines);

            if (densityOpt.HasValue
                && mineCountOpt.HasValue)
            {
                result.ErrorMessage = "It is not allowed to specify both a number of mines and density at the same time.";
            }

            if (widthOpt is not null)
                preset = preset with { Width = widthOpt.Value };

            if (heightOpt is not null)
                preset = preset with { Height = heightOpt.Value };

            if (mineCountOpt is not null)
                preset = preset with { MineCount = mineCountOpt.Value };

            if (useQuestionableOpt is not null)
                MinesweeperOptions.UseQuestionable = useQuestionableOpt.Value;

            if (noVerticalSeparatorOpt == true)
                MinesweeperOptions.VerticalSeparatorCellFormat = MinesweeperOptions.VerticalSeparatorCellFormat with { Character = '\0' };

            if (noRemainingMinesOpt == true)
                MinesweeperOptions.ShowRemainingMines = false;

            int width = preset.Width;
            int height = preset.Height;

            if (width < 0)
            {
                result.ErrorMessage = "Width must be greater than or equal to 0.";
                return;
            }

            int maxWidth = Console.WindowWidth;

            if (MinesweeperOptions.VerticalSeparatorCellFormat.Character != default)
                maxWidth /= 2;

            if (width > maxWidth)
            {
                result.ErrorMessage = "Field is too wide.";
                return;
            }

            if (width == 0)
                preset = preset with { Width = maxWidth };

            if (height < 0)
            {
                result.ErrorMessage = "Height must be greater than or equal to 0.";
                return;
            }

            int maxHeight = Console.WindowHeight - 1;

            if (MinesweeperOptions.ShowRemainingMines)
                maxHeight--;

            if (height > maxHeight)
            {
                result.ErrorMessage = "Field is too high.";
                return;
            }

            if (height == 0)
                preset = preset with { Height = maxHeight };

            if (densityOpt.HasValue)
                preset = preset with { MineCount = (int)(preset.Width * preset.Height * densityOpt.Value / 100.0) };

            int mineCount = preset.MineCount;

            if (mineCount < 0)
            {
                result.ErrorMessage = "Mine count must be greater than or equal to 0.";
                return;
            }

            int maxMineCount = preset.Width * preset.Height;

            if (mineCount > maxMineCount)
            {
                result.ErrorMessage = "Number of mines must be lower or equal to number of cells.";
                return;
            }

            if (mineCount == 0)
                preset = preset with { MineCount = (int)(maxMineCount * 0.15) };

            MinesweeperOptions.DefaultPreset = preset;
        });

        this.SetHandler(
            context =>
            {
                (MineFieldOptions fieldOptions, RenderOptions renderOptions) = MinesweeperOptions.Bind();

                var game = new MinesweeperGame(fieldOptions, renderOptions);

                context.ExitCode = (int)game.RunLoop();
            });
    }

    public Argument<string?> PresetArgument { get; }
    public Option<int?> WidthOption { get; }
    public Option<int?> HeightOption { get; }
    public Option<int?> MineCountOption { get; }
    public Option<int?> DensityOption { get; }
    public Option<bool?> QuestionableOption { get; }
    public Option<bool?> NoSeparatorOption { get; }
    public Option<bool?> NoRemainingMines { get; }
    public MinesweeperOptions MinesweeperOptions { get; }
}
