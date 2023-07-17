FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Mentor.API/Mentor.API.csproj", "Mentor.API/"]
COPY ["Mentor.Infrastructure/Mentor.Infrastructure.csproj", "Mentor.Infrastructure/"]
RUN dotnet restore "Mentor.API/Mentor.API.csproj"
COPY . .
WORKDIR "/src/Mentor.API"
RUN dotnet build "Mentor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mentor.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mentor.API.dll"]