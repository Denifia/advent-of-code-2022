param (
    [string]$year = $(Get-Date -UFormat "%Y"),
    [string]$day = $(Get-Date -UFormat "%d")
)

if(-not(Test-Path -Path $year\$day\$day.csproj -PathType Leaf))
{
    New-Item -ItemType Directory -Force -Path .\$year\$day
    Set-Location .\$year\$day
    dotnet new aoc
    Set-Location ..\..
}

Invoke-Item $year\$day\$day.csproj