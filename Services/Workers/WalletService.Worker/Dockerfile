FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Workers/WalletService.Worker/WalletService.Worker.csproj", "Services/Workers/WalletService.Worker/"]
RUN dotnet restore "Services/Workers/WalletService.Worker/WalletService.Worker.csproj"
COPY . .
WORKDIR "/src/Services/Workers/WalletService.Worker"
RUN dotnet build "WalletService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WalletService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.Worker.dll"]
