# Advent of Code

Pull the repo and install the template.
```ps
git clone https://github.com/Denifia/advent-of-code.git
.\install-template.ps1
```

Start today's challenge.
```ps
.\start-challenge.ps1
```

Start any challenge by specifying the year and day.
```ps
.\start-challenge.ps1 -year 2022 -day 01
```

Manual steps if needed.
```ps
dotnet new aoc -o 2024\01
dotnet sln add 2024\01\01.csproj
start advent-of-code.sln
```