using System.Text.Json;
using System.Text.Json.Serialization;

namespace Minesweeper;

public class ApplicationOptions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    public MinesweeperOptions Minesweeper { get; set; } = new();

    public static ApplicationOptions Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "config.json");

        return JsonSerializer.Deserialize<ApplicationOptions>(File.ReadAllText(path), _jsonSerializerOptions)!;
    }
}
