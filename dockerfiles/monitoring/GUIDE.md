# Monitoring Tools 

Tools for tracing the application.

## Overview

This stack provides a comprehensive solution for monitoring metrics and logs. Below is a brief explanation of each tool in the stack:

- **Grafana**: A **data visualization and monitoring platform**. It allows users to create dashboards and monitor data from various sources (such as Prometheus and Loki). Grafana provides visualization of time-series data with graphs and alerts.

- **Loki**: A **log aggregation system** designed to collect, store, and query logs. It integrates with Grafana to provide a unified interface for querying logs, following a similar approach to Prometheus but focused on log data.

- **Prometheus**: A system for **collecting and querying time-series data**, primarily used for monitoring and alerting. It gathers metrics from applications and systems, providing detailed insights into performance and health.

- **Promtail**: An **agent that sends logs to Loki**. It collects and forwards logs from applications or systems to Loki for storage and querying.

Together, these tools provide a unified platform for visualizing and analyzing your application's performance metrics and logs.


## Initializing the Monitoring Tool
Select the Local or Docker API setup to proceed
### Local API Setup
#### Loki Configuration
- In your monitoring environment file, add 
```
APP_LOGS=./../../src/Microservice/logs
```
- To enable human‑readable JSON in the dashboard, ensure that the following setting appears in both your appsettings.Development.json and appsettings.Production.json files 
```
"UseJsonFormat": true|false
```
#### Prometheus Configuration
- Before running docker-compose up, ensure the Docker network prometheus-net exists. Create it with
```
docker network create prometheus-net
```
- If you skip this step, docker-compose up will fail due to the missing network.

### Docker API Setup
#### Loki Configuration
- Remove setting from monitoring environment file `APP_LOGS=./../../src/Microservice/logs`
- To enable human‑readable JSON in the dashboard, ensure that the following setting appears in both your appsettings.Development.json and appsettings.Production.json files 
```
"UseJsonFormat": true|false
```
#### Prometheus Configuration
- Before running docker-compose up, ensure the Docker network prometheus-net and volume shared-logs exists. Create these with
```
docker network create prometheus-net
docker volume create shared-logs
```

We are now ready to run the Docker Compose file. Please execute the following command within the monitoring directory.

```
docker-compose up -d
```

## Configure Grafana

Use the following link to access the Grafana interface: [http://localhost:3000/login](http://localhost:3000/login). The default Grafana username and password are both `admin`.

In the **Connections/Data Sources** section, register Prometheus and Loki. For Loki, use the URL: `http://loki:3100`, and for Prometheus, use: `http://prometheus:9090`. After entering the URLs, click **Save & Test** for each source. You can now create dashboards, view logs, monitor performance, and more. Enjoy using your monitoring tools!

## Notes

- To filter logs, use the appropriate label and value (e.g., `job:varlogs`). Ensure that the application stores logs in JSON format to display logs correctly.

## Known Issues

- If the application is running locally (outside of Docker), you will not be able to use Prometheus to monitor performance or other metrics. This is due to a current limitation where the Prometheus container cannot access the host machine's URL via the local IP.
- To switch between the Docker and local setups, update the environment file, remove the containers, and then restart them.
