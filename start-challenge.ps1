param (
    [string]$year = $(Get-Date -UFormat "%Y"),
    [string]$day = $(Get-Date -UFormat "%d")
)
New-Item -ItemType Directory -Force -Path .\$year\$day
Set-Location .\$year\$day
dotnet new aoc
Invoke-Item .\$day.csproj
Set-Location ..\..