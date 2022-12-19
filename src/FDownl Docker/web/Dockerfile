FROM bitnami/aspnet-core:latest

USER fdownl

WORKDIR ~/src/FDownl

COPY ./appsettings.json ~/appsettings.json

RUN dotnet publish -c release

RUN rm -rf ~/old \
    if [ -d ~/app ]; then \
      mv ~/app ~/old \
    fi

RUN mv ~/bin/release/net6.0/publish ~/app && \
    cp ~/appsettings.json ~/app

USER root

COPY ./fdownl.service /etc/systemd/system/fdownl.service

RUN systemctl restart fdownl

