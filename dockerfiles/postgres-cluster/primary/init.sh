#!/bin/bash
set -e

chown -R postgres:postgres /var/lib/postgresql

echo "host replication all 0.0.0.0/0 md5" >> /var/lib/postgresql/data/pg_hba.conf
echo "host all all 0.0.0.0/0 md5" >> /var/lib/postgresql/data/pg_hba.conf

exec gosu postgres postgres -c config_file=/etc/postgresql/postgresql.conf
