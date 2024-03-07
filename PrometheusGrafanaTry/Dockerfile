#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
##EXPOSE 8080
##EXPOSE 8081
#ENV ASPNETCORE_HTTP_PORTS=5212
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["PrometheusGrafanaTry/PrometheusGrafanaTry.csproj", "PrometheusGrafanaTry/"]
#RUN dotnet restore "PrometheusGrafanaTry/PrometheusGrafanaTry.csproj"
#COPY . .
#WORKDIR "/src/PrometheusGrafanaTry"
#RUN dotnet build "PrometheusGrafanaTry.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "PrometheusGrafanaTry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "PrometheusGrafanaTry.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
#EXPOSE 8000
EXPOSE 5212

# Copy everything
COPY ./ ./

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "PrometheusGrafanaTry.dll"]
