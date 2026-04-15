# Dockerfile for HNG_Stage_0
# Multi-stage build to produce a small runtime image

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HNG_Stage_0/HNG_Stage_0.csproj", "HNG_Stage_0/"]
RUN dotnet restore "HNG_Stage_0/HNG_Stage_0.csproj"

COPY . .
WORKDIR /src/HNG_Stage_0
RUN dotnet publish "HNG_Stage_0.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "HNG_Stage_0.dll"]