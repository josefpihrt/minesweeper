dotnet clean "../src/Minesweeper/Minesweeper.csproj"

dotnet build "../src/Minesweeper/Minesweeper.csproj" -c Release -v n /p:RunCodeAnalysis=false /nr:false /m

if(!$?) { Read-Host; Exit }

dotnet pack -c Release --no-build -v normal "../src/Minesweeper/Minesweeper.csproj"

dotnet tool uninstall minesweeper.commandline -g

dotnet tool install minesweeper.commandline --version 1.0.0 -g --add-source "../src/Minesweeper/bin/Release"

Write-Host "DONE"
