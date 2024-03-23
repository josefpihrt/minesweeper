# Minesweeper.CommandLine

Play Minesweeper on the command line.

## Installation

```bash
dotnet tool install minesweeper.commandline -g
```

## Keybindings

| Key | Action |
| --- | --- |
| A | Open cell |
| S | Flag cell |
| Control+Arrow (Option+Arrow on Mac) | Jump to next unknown cell |
| Shift+Arrow | Expand selection |
| P | Pause game |
| Q | Cancel game |
| R | Re-render screen |

## Presets

| Name | Width | Height | Mines |
| --- | ---:| ---:| ---:|
| Beginner | 9 | 9 | 10 |
| Intermediate (default) | 16 | 16 | 40 |
| Expert | 30 | 16 | 99 |
