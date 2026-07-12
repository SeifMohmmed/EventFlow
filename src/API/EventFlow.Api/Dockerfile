FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/API/EventFlow.Api/EventFlow.Api.csproj", "src/API/EventFlow.Api/"]

COPY ["src/Common/EventFlow.Common.Application/EventFlow.Common.Application.csproj", "src/Common/EventFlow.Common.Application/"]
COPY ["src/Common/EventFlow.Common.Domain/EventFlow.Common.Domain.csproj", "src/Common/EventFlow.Common.Domain/"]
COPY ["src/Common/EventFlow.Common.Infrastructure/EventFlow.Common.Infrastructure.csproj", "src/Common/EventFlow.Common.Infrastructure/"]
COPY ["src/Common/EventFlow.Common.Presentation/EventFlow.Common.Presentation.csproj", "src/Common/EventFlow.Common.Presentation/"]

COPY ["src/Modules/Attendance/EventFlow.Modules.Attendance.Application/EventFlow.Modules.Attendance.Application.csproj", "src/Modules/Attendance/EventFlow.Modules.Attendance.Application/"]
COPY ["src/Modules/Attendance/EventFlow.Modules.Attendance.Domain/EventFlow.Modules.Attendance.Domain.csproj", "src/Modules/Attendance/EventFlow.Modules.Attendance.Domain/"]
COPY ["src/Modules/Attendance/EventFlow.Modules.Attendance.Infrastructure/EventFlow.Modules.Attendance.Infrastructure.csproj", "src/Modules/Attendance/EventFlow.Modules.Attendance.Infrastructure/"]
COPY ["src/Modules/Attendance/EventFlow.Modules.Attendance.IntegrationEvents/EventFlow.Modules.Attendance.IntegrationEvents.csproj", "src/Modules/Attendance/EventFlow.Modules.Attendance.IntegrationEvents/"]
COPY ["src/Modules/Attendance/EventFlow.Modules.Attendance.Presentation/EventFlow.Modules.Attendance.Presentation.csproj", "src/Modules/Attendance/EventFlow.Modules.Attendance.Presentation/"]

COPY ["src/Modules/Events/EventFlow.Modules.Events.Application/EventFlow.Modules.Events.Application.csproj", "src/Modules/Events/EventFlow.Modules.Events.Application/"]
COPY ["src/Modules/Events/EventFlow.Modules.Events.Domain/EventFlow.Modules.Events.Domain.csproj", "src/Modules/Events/EventFlow.Modules.Events.Domain/"]
COPY ["src/Modules/Events/EventFlow.Modules.Events.Infrastructure/EventFlow.Modules.Events.Infrastructure.csproj", "src/Modules/Events/EventFlow.Modules.Events.Infrastructure/"]
COPY ["src/Modules/Events/EventFlow.Modules.Events.IntegrationEvents/EventFlow.Modules.Events.IntegrationEvents.csproj", "src/Modules/Events/EventFlow.Modules.Events.IntegrationEvents/"]
COPY ["src/Modules/Events/EventFlow.Modules.Events.Presentation/EventFlow.Modules.Events.Presentation.csproj", "src/Modules/Events/EventFlow.Modules.Events.Presentation/"]

COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Application/EventFlow.Modules.Ticketing.Application.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Application/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Domain/EventFlow.Modules.Ticketing.Domain.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Domain/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Infrastructure/EventFlow.Modules.Ticketing.Infrastructure.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Infrastructure/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.IntegrationEvents/EventFlow.Modules.Ticketing.IntegrationEvents.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.IntegrationEvents/"]
COPY ["src/Modules/Ticketing/EventFlow.Modules.Ticketing.Presentation/EventFlow.Modules.Ticketing.Presentation.csproj", "src/Modules/Ticketing/EventFlow.Modules.Ticketing.Presentation/"]

COPY ["src/Modules/Users/EventFlow.Modules.Users.Application/EventFlow.Modules.Users.Application.csproj", "src/Modules/Users/EventFlow.Modules.Users.Application/"]
COPY ["src/Modules/Users/EventFlow.Modules.Users.Domain/EventFlow.Modules.Users.Domain.csproj", "src/Modules/Users/EventFlow.Modules.Users.Domain/"]
COPY ["src/Modules/Users/EventFlow.Modules.Users.Infrastructure/EventFlow.Modules.Users.Infrastructure.csproj", "src/Modules/Users/EventFlow.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/EventFlow.Modules.Users.IntegrationEvents/EventFlow.Modules.Users.IntegrationEvents.csproj", "src/Modules/Users/EventFlow.Modules.Users.IntegrationEvents/"]
COPY ["src/Modules/Users/EventFlow.Modules.Users.Presentation/EventFlow.Modules.Users.Presentation.csproj", "src/Modules/Users/EventFlow.Modules.Users.Presentation/"]

RUN dotnet restore "src/API/EventFlow.Api/EventFlow.Api.csproj"

COPY . .

WORKDIR "/src/src/API/EventFlow.Api"

RUN dotnet build EventFlow.Api.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish EventFlow.Api.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EventFlow.Api.dll"]