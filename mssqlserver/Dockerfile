FROM mcr.microsoft.com/mssql/server:2019-CU8-ubuntu-16.04
EXPOSE 1433
USER root

COPY import-data.sh import-data.sh
COPY docker-entrypoint.sh docker-entrypoint.sh
COPY . .
# Remove DOS line endings (CRLF), remove CR: \r and and leave only LF: \n
RUN sed -i 's/\r//' /import-data.sh
RUN sed -i 's/\r//' /docker-entrypoint.sh

RUN chmod +x docker-entrypoint.sh

ENTRYPOINT ["/docker-entrypoint.sh"]
