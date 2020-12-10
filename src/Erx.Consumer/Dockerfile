#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY ["src/Erx.Consumer/Erx.Consumer.csproj", "src/Erx.Consumer/"]
RUN dotnet restore "src/Erx.Consumer/Erx.Consumer.csproj"
COPY . .
WORKDIR "/src/src/Erx.Consumer"
RUN dotnet build "Erx.Consumer.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "Erx.Consumer.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erx.Consumer.dll"]
