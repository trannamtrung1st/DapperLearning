FROM mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04 AS base
EXPOSE 1433

RUN mkdir -p /usr/init
WORKDIR /usr/init

COPY misc/database/ /usr/init

USER root

RUN chmod +x /usr/init/entrypoint.sh
RUN chmod +x /usr/init/init.sh

ENTRYPOINT ["./entrypoint.sh"]