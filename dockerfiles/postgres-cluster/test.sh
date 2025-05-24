#!/bin/bash
set -e

echo "▶️  Starting PostgreSQL Replication Test..."

# 1. Insert data into PRIMARY
echo "🔹 Inserting data into primary..."
docker exec -i pg-primary psql -U admin -d admin <<EOF
DROP TABLE IF EXISTS test_table;
CREATE TABLE test_table (id serial PRIMARY KEY, message text);
INSERT INTO test_table (message) VALUES ('replica test 1'), ('replica test 2');
EOF

# 2. Read data from REPLICA 1
echo "🔹 Reading data from replica 1..."
replica_output=$(docker exec -i pg-replica-1 psql -U admin -d admin -t -c "SELECT * FROM test_table ORDER BY id;")

# 3. Read data from REPLICA 2
echo "🔹 Reading data from replica 2..."
replica2_output=$(docker exec -i pg-replica-2 psql -U admin -d admin -t -c "SELECT * FROM test_table ORDER BY id;")

# 4. Read data via PGPOOL
echo "🔹 Reading data via pgpool..."
pgpool_output=$(docker exec -i pgpool psql -h pg-primary -U admin -d admin -t -c "SELECT * FROM test_table ORDER BY id;")

# 5. Comparison
echo "🧪 Test Results:"
echo "Replica 1:"
echo "$replica_output"

echo "Replica 2:"
echo "$replica2_output"

echo "Pgpool:"
echo "$pgpool_output"

if [[ "$replica_output" == "$replica2_output" && "$replica_output" == "$pgpool_output" ]]; then
  echo "✅ ALL TESTS PASSED: Replicas and pgpool outputs match!"
else
  echo "❌ ERROR: Data mismatch detected, replication may be faulty."
  exit 1
fi
