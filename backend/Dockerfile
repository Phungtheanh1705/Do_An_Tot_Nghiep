FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/LocalExplore.Domain/LocalExplore.Domain.csproj", "LocalExplore.Domain/"]
COPY ["src/LocalExplore.Application/LocalExplore.Application.csproj", "LocalExplore.Application/"]
COPY ["src/LocalExplore.Infrastructure/LocalExplore.Infrastructure.csproj", "LocalExplore.Infrastructure/"]
COPY ["src/LocalExplore.API/LocalExplore.API.csproj", "LocalExplore.API/"]

RUN dotnet restore "LocalExplore.API/LocalExplore.API.csproj"

COPY src/ .
WORKDIR "/src/LocalExplore.API"
RUN dotnet publish "LocalExplore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5249
ENV ASPNETCORE_URLS=http://+:5249
ENTRYPOINT ["dotnet", "LocalExplore.API.dll"]
