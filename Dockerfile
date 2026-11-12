FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["FinTrack.slnx", "./"]
COPY [".config/dotnet-tools.json", ".config/"]
COPY ["src/FinTrack.Core/FinTrack.Core.csproj", "src/FinTrack.Core/"]
COPY ["src/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj", "src/FinTrack.Infrastructure/"]
COPY ["tests/FinTrack.Tests/FinTrack.Tests.csproj", "tests/FinTrack.Tests/"]
RUN dotnet restore FinTrack.slnx

COPY . ./
RUN dotnet tool restore
RUN dotnet build FinTrack.slnx --configuration Release --no-restore
RUN dotnet publish src/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj \
    --configuration Release \
    --no-build \
    --output /out/app

FROM build AS migrator
WORKDIR /src
ENTRYPOINT ["dotnet", "tool", "run", "dotnet-ef", "database", "update", "--project", "src/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj", "--startup-project", "src/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj", "--configuration", "Release", "--no-build"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /out/app ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinTrack.Infrastructure.dll"]
