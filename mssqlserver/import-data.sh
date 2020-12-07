#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
set -e

password=${SA_PASSWORD:-Authapp@123}
database=${DATABASE:-authapp_db}

for i in {1..50}
do
   /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$password" -d "$database" -i setup.sql
   if  [$? -eq 0]
   then
      echo "setup.sql completed"
      break
   else 
      echo "not ready yet..."
      sleep 1
   fi
done