#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Erx.Api/Erx.Api.csproj", "src/Erx.Api/"]
RUN dotnet restore "src/Erx.Api/Erx.Api.csproj"
COPY . .
WORKDIR "/src/src/Erx.Api"
RUN dotnet build "Erx.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erx.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erx.Api.dll"]