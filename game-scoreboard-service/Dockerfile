#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["game-scoreboard-service/game-scoreboard-service.csproj", "game-scoreboard-service/"]
RUN dotnet restore "game-scoreboard-service/game-scoreboard-service.csproj"
COPY . .
WORKDIR "/src/game-scoreboard-service"
RUN dotnet build "game-scoreboard-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "game-scoreboard-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "game-scoreboard-service.dll"]