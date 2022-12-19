FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG fdownlstorage="./src/Fdownl Storage"
ADD ${fdownlstorage} /home/fdownl/fdownlstorage

ARG fdownlshared="./src/FDownl Shared Resources"
ARG localshared="/home/fdownl/FDownl Shared Resources"
ADD ${fdownlshared} ${localshared}

WORKDIR /home/fdownl/fdownlstorage

RUN dotnet publish -c release

USER fdownl

RUN cp -r ./bin/release/net?.0/publish ~/app

COPY ./storage/appsettings.json ~/app/appsettings.json

ENTRYPOINT ["/usr/bin/dotnet", "/home/fdownl/app/Fdownl Storage.dll"]
