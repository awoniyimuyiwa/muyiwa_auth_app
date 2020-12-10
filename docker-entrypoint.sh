#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
#set -e

=${SERVER:?"SERVER environment variable must be set"}

/wait-for-it.sh "$server:1433"

for file in /scripts/*.sql; do
    /opt/mssql-tools/bin/sqlcmd -S "$server" -U sa -P "$password" -d "$database" -i "$file"
    if  [$? -eq 0]
    then
       echo "Exceuted $file"
       break
    else 
        sleep 1
    fi
done

echo "Database setup completed"