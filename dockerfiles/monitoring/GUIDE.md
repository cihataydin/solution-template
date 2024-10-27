# Monitoring Tools 

Tools for tracing the service.

## Overview

This stack provides a comprehensive solution for monitoring metrics and logs. Below is a brief explanation of each tool in the stack:

- **Grafana**: A **data visualization and monitoring platform**. It allows users to create dashboards and monitor data from various sources (such as Prometheus and Loki). Grafana provides visualization of time-series data with graphs and alerts.

- **Loki**: A **log aggregation system** designed to collect, store, and query logs. It integrates with Grafana to provide a unified interface for querying logs, following a similar approach to Prometheus but focused on log data.

- **Prometheus**: A system for **collecting and querying time-series data**, primarily used for monitoring and alerting. It gathers metrics from applications and systems, providing detailed insights into performance and health.

- **Promtail**: An **agent that sends logs to Loki**. It collects and forwards logs from applications or systems to Loki for storage and querying.

Together, these tools provide a unified platform for visualizing and analyzing your application's performance metrics and logs.


## Initialize

The service can be run using the Docker Compose file immediately. However, if the Dockerized API is intended to be used for tracing, it is necessary to first create the required volume and network. Since there are separate Docker Compose files, the network and volume must be shared between them. Also the `APP_LOGS` parameter in the `.env` file should be commented out. This is necessary to ensure the service uses the created volume for log storage.

If you have started the application locally using `dotnet run` without Docker, you may skip this step and directly run the Docker Compose file. Simply ensure that the `APP_LOGS` parameter in the `.env` file is correctly set to the appropriate path.

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

In the **Data Sources** section, register Prometheus and Loki. For Loki, use the URL: `http://loki:3100`, and for Prometheus, use: `http://prometheus:9090`. After entering the URLs, click **Save & Test** for each source. You can now create dashboards, view logs, monitor performance, and more. Enjoy using your monitoring tools!

## Notes

- To filter logs, use the appropriate label and value (e.g., `job:varlogs`). Ensure that the application stores logs in JSON format to display logs correctly.

## Known Issues

- If the application is running locally (outside of Docker), you will not be able to use Prometheus to monitor performance or other metrics. This is due to a current limitation where the Prometheus container cannot access the host machine's URL via the local IP.
