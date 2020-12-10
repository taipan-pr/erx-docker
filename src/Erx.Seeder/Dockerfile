#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY ["src/Erx.Seeder/Erx.Seeder.csproj", "src/Erx.Seeder/"]
RUN dotnet restore "src/Erx.Seeder/Erx.Seeder.csproj"
COPY . .
WORKDIR "/src/src/Erx.Seeder"
RUN dotnet build "Erx.Seeder.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Erx.Seeder.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erx.Seeder.dll"]
