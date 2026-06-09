# ==========================================
# 1. BUILD STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copier le fichier de solution et les projets (optimisation du cache Docker)
COPY ["TradeOffStackAPI.sln", "./"]
COPY ["TradeOffStackAPI/TradeOffStackAPI.csproj", "TradeOffStackAPI/"]
COPY ["TradeOffStackAPI.Tests/TradeOffStackAPI.Tests.csproj", "TradeOffStackAPI.Tests/"]

# Restaurer les dépendances
RUN dotnet restore "TradeOffStackAPI.sln"

# Copier tout le code source
COPY . .

# Compiler et publier l'API en Release
WORKDIR "/src/TradeOffStackAPI"
RUN dotnet publish "TradeOffStackAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# 2. RUNTIME STAGE (Production)
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Sécurité : utiliser l'utilisateur non-root 'app' intégré dans les images .NET 8+
USER app

# Copier les fichiers compilés depuis le build stage
COPY --from=build /app/publish .

# Définir le port d'écoute (8080 est le standard par défaut dans .NET 8+)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Lancer l'application
ENTRYPOINT ["dotnet", "TradeOffStackAPI.dll"]
