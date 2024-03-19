// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Minesweeper;

public class MineFieldOptions
{
    private int _mineCount = 40;
    private int _width = 16;
    private int _height = 16;

    public bool UseQuestionMark { get; set; }

    public int Width
    {
        get { return _width; }
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), value, "");

            _width = value;
        }
    }

    public int Height
    {
        get { return _height; }
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), value, "");

            _height = value;
        }
    }

    public int MineCount
    {
        get { return _mineCount; }
        set
        {
            if (value < 1
                || value > Width * Height)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "");
            }

            _mineCount = value;
        }
    }
}
