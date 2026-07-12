# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/API/EventFlow.Ticketing.Api/EventFlow.Ticketing.Api.csproj", "src/API/EventFlow.Ticketing.Api/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Infrastructure/EventFlow.Modules.Ticketing.Infrastructure.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Infrastructure/"]
COPY ["src/Common/EventFlow.Common.Infrastructure/EventFlow.Common.Infrastructure.csproj", "src/Common/EventFlow.Common.Infrastructure/"]
COPY ["src/Common/EventFlow.Common.Application/EventFlow.Common.Application.csproj", "src/Common/EventFlow.Common.Application/"]
COPY ["src/Common/EventFlow.Common.Domain/EventFlow.Common.Domain.csproj", "src/Common/EventFlow.Common.Domain/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Application/EventFlow.Modules.Ticketing.Application.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Application/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Domain/EventFlow.Modules.Ticketing.Domain.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Domain/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.IntegrationEvents/EventFlow.Modules.Ticketing.IntegrationEvents.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.IntegrationEvents/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Presentation/EventFlow.Modules.Ticketing.Presentation.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Presentation/"]
COPY ["src/Common/EventFlow.Common.Presentation/EventFlow.Common.Presentation.csproj", "src/Common/EventFlow.Common.Presentation/"]
COPY ["src/Modules/Events/EventFlow.Modules.Events.IntegrationEvents/EventFlow.Modules.Events.IntegrationEvents.csproj", "src/Modules/Events/EventFlow.Modules.Events.IntegrationEvents/"]
COPY ["src/Modules/Users/EventFlow.Modules.Users.IntegrationEvents/EventFlow.Modules.Users.IntegrationEvents.csproj", "src/Modules/Users/EventFlow.Modules.Users.IntegrationEvents/"]
RUN dotnet restore "./src/API/EventFlow.Ticketing.Api/EventFlow.Ticketing.Api.csproj"
COPY . .
WORKDIR "/src/src/API/EventFlow.Ticketing.Api"
RUN dotnet build "./EventFlow.Ticketing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventFlow.Ticketing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventFlow.Ticketing.Api.dll"]