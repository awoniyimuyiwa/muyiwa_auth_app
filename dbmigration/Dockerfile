FROM mcr.microsoft.com/mssql-tools
USER root

COPY docker-entrypoint.sh docker-entrypoint.sh
COPY wait-for-it.sh wait-for-it.sh
COPY . .
# Remove DOS line endings (CRLF), remove CR: \r and and leave only LF: \n
RUN sed -i 's/\r//' /docker-entrypoint.sh
RUN sed -i 's/\r//' /wait-for-it.sh

RUN chmod +x ./docker-entrypoint.sh ./wait-for-it.sh

ENTRYPOINT ["/docker-entrypoint.sh"]