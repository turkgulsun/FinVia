FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Workers/KycService.Worker/KycService.Worker.csproj", "Services/Workers/KycService.Worker/"]
RUN dotnet restore "Services/Workers/KycService.Worker/KycService.Worker.csproj"
COPY . .
WORKDIR "/src/Services/Workers/KycService.Worker"
RUN dotnet build "KycService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KycService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KycService.Worker.dll"]
