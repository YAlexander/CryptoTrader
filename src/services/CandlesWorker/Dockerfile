FROM mcr.microsoft.com/dotnet/core/runtime:3.0-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch AS build
WORKDIR /src
COPY ["src/Services/CandlesWorker/CandlesWorker.csproj", "src/Services/CandlesWorker/"]
RUN dotnet restore "src/Services/CandlesWorker/CandlesWorker.csproj"
COPY . .
WORKDIR "/src/src/Services/CandlesWorker"
RUN dotnet build "CandlesWorker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CandlesWorker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CandlesWorker.dll"]