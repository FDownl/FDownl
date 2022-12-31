FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN useradd -ms /bin/bash fdownl

USER root

ARG src="./src/"
ADD ${src} /home/fdownl/src

WORKDIR /home/fdownl/src/Fdownl\ Storage

RUN dotnet publish -c release

USER fdownl

RUN cp -r ./bin/release/net?.0/publish ~/app

COPY ./storage/appsettings.json ~/app/appsettings.json

CMD /usr/bin/dotnet /home/fdownl/app/Fdownl\ Storage.dll
