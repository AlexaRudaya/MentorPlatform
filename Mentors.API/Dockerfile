FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Mentors.API/Mentors.API.csproj", "Mentors.API/"]
COPY ["Mentors.ApplicationCore/Mentors.ApplicationCore.csproj", "Mentors.ApplicationCore/"]
COPY ["Mentors.Domain/Mentors.Domain.csproj", "Mentors.Domain/"]
COPY ["Mentors.Infrastructure/Mentors.Infrastructure.csproj", "Mentors.Infrastructure/"]
RUN dotnet restore "Mentors.API/Mentors.API.csproj"
COPY . .
WORKDIR "/src/Mentors.API"
RUN dotnet build "Mentors.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mentors.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mentors.API.dll"]