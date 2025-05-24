# PostgreSQL + Pgpool Replication

This project sets up a PostgreSQL replication environment using Docker Compose. It includes:
- One **Primary** PostgreSQL node
- Two **Replicas**
- A **Pgpool** node for load balancing and (future) failover

It also includes an automated test script to verify replication is working as expected.

---

## ✅ Features

- PostgreSQL 16 replication (streaming)
- Two replicas kept in sync with the primary
- Pgpool for centralized connection management
- Automatic test script to insert and verify data
- Fully Dockerized and isolated environment

---

## 🛠️ Setup Instructions (Step-by-Step)

### 1. Clean Previous Setup

Remove existing containers, volumes, and the custom Docker network (if you’ve run the system before):

```bash
docker compose down -v       # Stops and removes containers and volumes
docker network rm pgnet      # Removes the external network
```

This command starts all services in the background:

```bash
docker network create pgnet && docker compose up --build -d
```

### 2. Test

```bash
chmod +x test.sh
````

```bash
./test.sh
```

This script will:

- Connect to the Primary and insert test data into a table

- Query the Replicas and Pgpool

- Compare the data across all nodes

- Output the result of the verification
