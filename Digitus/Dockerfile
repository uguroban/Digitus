FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Digitus/Digitus.csproj", "Digitus/"]
RUN dotnet restore "Digitus/Digitus.csproj"
COPY . .
WORKDIR "/src/Digitus"
RUN dotnet build "Digitus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Digitus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Digitus.dll"]
