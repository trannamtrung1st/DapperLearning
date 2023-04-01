#!/bin/bash

if [ ! -f /usr/init/flag ]; then
    touch flag &
    /usr/init/init.sh &
    /opt/mssql/bin/sqlservr
else
    /opt/mssql/bin/sqlservr
fi