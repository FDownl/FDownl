FROM bitnami/aspnet-core:latest

USER fdownl

WORKDIR ~/src/Fdownl\ Storage

COPY ./appsettings.json ~/appsettings.json

RUN dotnet publish -c release

RUN rm -rf ~/old \
    if [ -d ~/app ]; then \
      mv ~/app ~/old \
    fi

RUN mv ~/bin/release/net6.0/publish ~/app && \
    cp ~/appsettings.json ~/app

USER root

COPY ./fdownlstorage.service /etc/systemd/system/fdownlstorage.service

RUN systemctl restart fdownlstorage

