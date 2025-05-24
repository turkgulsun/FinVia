FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/KycService/KycService.API/KycService.API.csproj", "Services/KycService/KycService.API/"]
RUN dotnet restore "Services/KycService/KycService.API/KycService.API.csproj"
COPY . .
WORKDIR "/src/Services/KycService/KycService.API"
RUN dotnet build "KycService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KycService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KycService.API.dll"]
