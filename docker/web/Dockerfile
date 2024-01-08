FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG src="./src/"
ADD ${src} /home/fdownl/src

WORKDIR /home/fdownl/src/FDownl

RUN dotnet restore --interactive -s https://api.nuget.org/v3/index.json

RUN dotnet publish -c release

RUN cp -r ./bin/release/net?.0/publish /app

WORKDIR /app

COPY ./docker/web/appsettings.json ./appsettings.json

USER root

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["/usr/bin/dotnet", "/app/FDownl.dll"]
