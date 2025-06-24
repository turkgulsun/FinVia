FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Workers/CampaignService.OutboxPublisherWorker/CampaignService.OutboxPublisherWorker.csproj", "Services/Workers/CampaignService.OutboxPublisherWorker/"]
RUN dotnet restore "Services/Workers/CampaignService.OutboxPublisherWorker/CampaignService.OutboxPublisherWorker.csproj"
COPY . .
WORKDIR "/src/Services/Workers/CampaignService.OutboxPublisherWorker"
RUN dotnet build "CampaignService.OutboxPublisherWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CampaignService.OutboxPublisherWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignService.OutboxPublisherWorker.dll"]
