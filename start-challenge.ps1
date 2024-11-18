param (
    [string]$year = $(Get-Date -UFormat "%Y"),
    [string]$day = $(Get-Date -UFormat "%d")
)

dotnet new aoc -o 2024\01
dotnet sln add 2024\01\01.csproj

Invoke-Item advent-of-code.sln