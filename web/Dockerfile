FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG fdownl="./src/FDownl"
ADD ${fdownl} /home/fdownl/fdownl

ARG fdownlshared="./src/FDownl Shared Resources"
ARG localshared="/home/fdownl/FDownl Shared Resources"
ADD ${fdownlshared} ${localshared}

WORKDIR /home/fdownl/fdownl

RUN dotnet publish -c release

USER fdownl

RUN cp -r ./bin/release/net?.0/publish ~/app

COPY ./web/appsettings.json ~/app/appsettings.json

ENTRYPOINT ["/usr/bin/dotnet", "/home/fdownl/app/FDownl.dll"]
