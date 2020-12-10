#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
#set -e

server=${SERVER:?"SERVER environment variable must be set"}
password=${SA_PASSWORD:?"SA_PASSWORD environment variable must be set"}
timeout=${TIMEOUT:-300} 

/wait-for-it.sh -t "$timeout" "$server:1433"
if  [$? -ne 0] 
then
    echo >&2 "Unable to connect to $server. Aborting."
    exit
fi   

for file in /scripts/*.sql; do
    /opt/mssql-tools/bin/sqlcmd -S "$server" -U sa -P "$password" -d master -i "$file"
    if  [$? -eq 0]
    then
       echo "Exceuted $file"
       break
    else 
        sleep 1
    fi
done

echo "Database setup completed"