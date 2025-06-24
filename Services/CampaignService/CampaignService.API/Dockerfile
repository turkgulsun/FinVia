FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/CampaignService/CampaignService.API/CampaignService.API.csproj", "Services/CampaignService/CampaignService.API/"]
RUN dotnet restore "Services/CampaignService/CampaignService.API/CampaignService.API.csproj"
COPY . .
WORKDIR "/src/Services/CampaignService/CampaignService.API"
RUN dotnet build "CampaignService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CampaignService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignService.API.dll"]
