FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Workers/CampaignService.Worker/CampaignService.Worker.csproj", "Services/Workers/CampaignService.Worker/"]
RUN dotnet restore "Services/Workers/CampaignService.Worker/CampaignService.Worker.csproj"
COPY . .
WORKDIR "/src/Services/Workers/CampaignService.Worker"
RUN dotnet build "CampaignService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CampaignService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignService.Worker.dll"]
