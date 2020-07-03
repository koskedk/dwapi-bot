#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Dwapi.Bot/Dwapi.Bot.csproj", "src/Dwapi.Bot/"]
COPY ["src/Dwapi.Bot.Infrastructure/Dwapi.Bot.Infrastructure.csproj", "src/Dwapi.Bot.Infrastructure/"]
COPY ["src/Dwapi.Bot.Core/Dwapi.Bot.Core.csproj", "src/Dwapi.Bot.Core/"]
COPY ["src/Dwapi.Bot.SharedKernel/Dwapi.Bot.SharedKernel.csproj", "src/Dwapi.Bot.SharedKernel/"]
RUN dotnet restore "src/Dwapi.Bot/Dwapi.Bot.csproj"
COPY . .
WORKDIR "/src/src/Dwapi.Bot"
RUN dotnet build "Dwapi.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dwapi.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dwapi.Bot.dll"]