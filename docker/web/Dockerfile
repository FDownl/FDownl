FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG src="./src/"
ADD ${src} /home/fdownl/src

WORKDIR /home/fdownl/src/FDownl

RUN dotnet restore --interactive -s https://api.nuget.org/v3/index.json

RUN dotnet publish -c release

USER fdownl

RUN cp -r ./bin/release/net?.0/publish ~/app

WORKDIR ~/app

COPY ./docker/web/appsettings.json ./appsettings.json

USER root

ADD https://github.com/vishnubob/wait-for-it/raw/master/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

ENTRYPOINT ["/wait-for-it.sh", "fdownl_db:3306", "-t", "30", "--", "/usr/bin/dotnet", "/home/fdownl/app/FDownl.dll"]
