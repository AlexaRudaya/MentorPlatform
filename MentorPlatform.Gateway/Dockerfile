FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7008
EXPOSE 7008

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MentorPlatform.Gateway/MentorPlatform.Gateway.csproj", "MentorPlatform.Gateway/"]
RUN dotnet restore "MentorPlatform.Gateway/MentorPlatform.Gateway.csproj"
COPY . .
WORKDIR "/src/MentorPlatform.Gateway"
RUN dotnet build "MentorPlatform.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MentorPlatform.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MentorPlatform.Gateway.dll"]