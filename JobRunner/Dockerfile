FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY . .
RUN dotnet restore JobRunner/JobRunner.csproj
WORKDIR /src/JobRunner
RUN dotnet build JobRunner.csproj -c Release -o /app


FROM build AS publish
RUN dotnet publish JobRunner.csproj -c Release -o /app
COPY JobRunner/Resources /app

FROM microsoft/dotnet:2.1-runtime-bionic AS runtime
WORKDIR /app
COPY --from=publish /app .


FROM runtime AS git-ftp
RUN apt-get update && \
    apt-get install -y software-properties-common
   
RUN add-apt-repository -y ppa:git-ftp/ppa && \
    apt-get update && \
    apt-get install -y git-ftp ssh

RUN mkdir -p ~/.ssh/ && ssh-keyscan github.com > ~/.ssh/known_hosts

#CMD exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
ENTRYPOINT ["dotnet", "Gitloy.Services.JobRunner.dll"]