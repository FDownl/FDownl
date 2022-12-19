FROM okteto/dotnetcore:3

RUN useradd -ms /bin/bash fdownl

USER root

ARG fdownl="./src/FDownl"
ADD ${fdownl} /home/fdownl/fdownl

COPY ./web/appsettings.json ~/appsettings.json

WORKDIR /home/fdownl/fdownl

RUN dotnet publish -c release

RUN rm -rf ~/old \
    if [ -d ~/app ]; then \
      mv ~/app ~/old \
    fi

RUN mv ~/bin/release/net?.0/publish ~/app && \
    cp ~/appsettings.json ~/app

COPY ./web/fdownl.service /etc/systemd/system/fdownl.service

RUN systemctl restart fdownl

ENTRYPOINT /home/fdownl
