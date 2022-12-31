FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG src="./src/"
ADD ${src} /home/fdownl/src

WORKDIR /home/fdownl/src/FDownl

RUN dotnet publish -c release

USER fdownl

RUN cp -r ./bin/release/net?.0/publish ~/app

COPY ./web/appsettings.json ~/app/appsettings.json

CMD /usr/bin/dotnet /home/fdownl/app/FDownl.dll
