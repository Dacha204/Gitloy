FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 2524
EXPOSE 44348

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore FrontPortal/FrontPortal.csproj
WORKDIR /src/FrontPortal
RUN dotnet build FrontPortal.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FrontPortal.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gitloy.Services.FrontPortal.dll"]
