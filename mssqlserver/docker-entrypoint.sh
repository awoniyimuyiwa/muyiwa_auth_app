#!/usr/bin/env bash

# Set bash script processing to fail on the first encountered error
set -e

./import-data.sh & /opt/mssql/bin/sqlserver