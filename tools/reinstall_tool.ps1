dotnet clean "../src/Minesweeper.Cli/Minesweeper.Cli.csproj"

dotnet build "../src/Minesweeper.Cli/Minesweeper.Cli.csproj" -c Release -v n /p:RunCodeAnalysis=false /nr:false /m

if(!$?) { Read-Host; Exit }

dotnet pack -c Release --no-build -v normal "../src/Minesweeper.Cli/Minesweeper.Cli.csproj"

dotnet tool uninstall dotnet-minesweeper -g

dotnet tool install dotnet-minesweeper --version 1.0.0 -g --add-source "../src/Minesweeper.Cli/bin/Release"

Write-Host "DONE"
