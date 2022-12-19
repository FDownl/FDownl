FROM okteto/dotnetcore:3

RUN useradd -ms /bin/bash fdownl

USER root

ARG fdownlstorage="./src/Fdownl Storage"
ADD ${fdownlstorage} /home/fdownl/fdownlstorage

COPY ./storage/appsettings.json ~/appsettings.json

WORKDIR /home/fdownl/fdownlstorage

RUN dotnet publish -c release

RUN rm -rf ~/old \
    if [ -d ~/app ]; then \
      mv ~/app ~/old \
    fi

RUN mv ~/bin/release/net?.0/publish ~/app && \
    cp ~/appsettings.json ~/app

COPY ./storage/fdownlstorage.service /etc/systemd/system/fdownlstorage.service

RUN systemctl restart fdownlstorage

ENTRYPOINT /home/fdownl
