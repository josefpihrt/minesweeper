dotnet build "../src/DotNetGame.Cli/DotNetGame.Cli.csproj" -c Debug -v n /p:RunCodeAnalysis=false /nr:false /m

if(!$?) { Read-Host; Exit }

dotnet pack -c Debug --no-build -v normal "../src/DotNetGame.Cli/DotNetGame.Cli.csproj"

dotnet tool uninstall dotnet-game -g

dotnet tool install dotnet-game --version 1.0.0 -g --add-source "../src/DotNetGame.Cli/bin/Debug"

Write-Host "DONE"
