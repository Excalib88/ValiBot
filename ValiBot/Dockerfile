FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ValiBot/ValiBot.csproj", "ValiBot/"]
RUN dotnet restore "ValiBot/ValiBot.csproj"
COPY . .
WORKDIR "/src/ValiBot"
RUN dotnet build "ValiBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValiBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValiBot.dll"]
