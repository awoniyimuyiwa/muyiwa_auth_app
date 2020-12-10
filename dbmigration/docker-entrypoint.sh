#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
set -e

server=${SERVER:?"SERVER environment variable must be set"}
password=${SA_PASSWORD:?"SA_PASSWORD environment variable must be set"}
timeout=${TIMEOUT:-300} 

if /wait-for-it.sh -t "$timeout" "$server:1433" 
then
    echo >&2 "Unable to connect to $server. Aborting."
    exit 1
fi   

for file in /scripts/*.sql
do
    /opt/mssql-tools/bin/sqlcmd -S "$server" -U sa -P "$password" -d master -i "$file"
    echo "Exceuted $file"
done

echo "Database setup completed"