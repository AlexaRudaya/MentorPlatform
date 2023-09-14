FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chat.API/Chat.API.csproj", "./Chat.API/"]
COPY ["../Chat.Infrastructure/Chat.Infrastructure.csproj", "Chat.Infrastructure/"]
COPY ["../Chat.Domain/Chat.Domain.csproj", "Chat.Domain/"]
RUN dotnet restore "Chat.API/Chat.API.csproj"
COPY . .
WORKDIR "/src/Chat.API"
RUN dotnet build "Chat.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.API.dll"]