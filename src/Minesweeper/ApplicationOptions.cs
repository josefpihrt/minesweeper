// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
