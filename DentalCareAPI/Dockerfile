FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["DentalCareAPI/DentalCareAPI.csproj", "DentalCareAPI/"]
RUN dotnet restore "DentalCareAPI/DentalCareAPI.csproj"
COPY . .
RUN dotnet publish "DentalCareAPI/DentalCareAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DentalCareAPI.dll"]
