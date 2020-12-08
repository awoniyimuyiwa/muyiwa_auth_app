#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
set -e

database=${DATABASE:?"DATABASE environment variable must be set"}
password=${SA_PASSWORD:?"SA_PASSWORD environment variable must be set"}

for i in {1..50}

for f in ./scripts/*.sql  do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$password" -d "$database" -i $f
  if  [$? -eq 0]
  then
    echo "$f setup completed"
    break
  else 
    echo "$f not ready yet..."
    sleep 1
   fi
done

echo "sql setup completed"