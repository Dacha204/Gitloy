FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore WebhookAPI/WebhookAPI.csproj
WORKDIR /src/WebhookAPI
RUN dotnet build WebhookAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebhookAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#CMD exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
ENTRYPOINT ["dotnet", "Gitloy.Services.WebhookAPI.dll"]