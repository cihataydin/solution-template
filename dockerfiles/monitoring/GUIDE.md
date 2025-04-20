# Monitoring Tools 

Tools for tracing the application.

## Contents
- [Overview](#overview)
- [Configure the Monitoring Tools](#configure-the-monitoring-tools)
    - [Local API Setup](#local-api-setup)
        - [Loki Configuration](#loki-configuration)
        - [Prometheus Configuration](#prometheus-configuration)
    - [Docker API Setup](#docker-api-setup)
        - [Loki Configuration](#loki-configuration-1)
        - [Prometheus Configuration](#prometheus-configuration-1)
- [Initialize the Monitoring Tools](#initialize-the-monitoring-tools)
- [Configure Grafana](#configure-grafana)
    - [Notes](#notes)
- [Known Issues](#known-issues)

## Overview
This stack provides a comprehensive solution for monitoring metrics and logs. Together, these tools provide a unified platform for visualizing and analyzing your application's performance metrics and logs.

## Configure the Monitoring Tools
Select the Local or Docker API setup to proceed
### Local API Setup
#### Loki Configuration
- In your monitoring environment file, add 
```
APP_LOGS=./../../src/Microservice/logs
```
- To enable human‑readable JSON in the dashboard, ensure that the following setting appears in both your `appsettings.Development.json` and `appsettings.Production.json` files 
```
"UseJsonFormat": true|false
```
#### Prometheus Configuration
- Before running docker-compose up, ensure the Docker network `prometheus-net` exists. Create it with
```
docker network create prometheus-net
```
- If you skip this step, docker-compose up will fail due to the missing network.

### Docker API Setup
#### Loki Configuration
- Remove setting from monitoring environment file `APP_LOGS=./../../src/Microservice/logs`
- To enable human‑readable JSON in the dashboard, ensure that the following setting appears in both your `appsettings.Development.json` and `appsettings.Production.json` files 
```
"UseJsonFormat": true|false
```
#### Prometheus Configuration
- Before running docker-compose up, ensure the Docker network `prometheus-net` and volume `shared-logs` exists. Create these with
```
docker network create prometheus-net
docker volume create shared-logs
```
## Initialize the Monitoring Tools
We are now ready to run the Docker Compose file. Please execute the following command within the `monitoring` directory.
```
docker-compose up -d
```

## Configure Grafana
Use the following link to access the Grafana interface: [http://localhost:3000/login](http://localhost:3000/login). The default Grafana username and password are both `admin`.

In the **Connections/Data Sources** section, register Prometheus and Loki. 
- For Loki, use the URL: `http://loki:3100`, 
- For Prometheus, use: `http://prometheus:9090`. 

After entering the URLs, click **Save & Test** for each source. You can now create dashboards, view logs, monitor performance, and more. Enjoy using your monitoring tools!

### Notes
- To filter logs, use the appropriate label and value (e.g., `job:varlogs`). Ensure that the application stores logs in JSON format to display logs correctly.

## Known Issues
- To switch between the Docker and local setups, update the environment file, remove the containers, and then restart them.

## Technologies and References
- [Grafana](https://grafana.com/)
- [Loki](https://grafana.com/oss/loki/)
- [Promtail Agent](https://grafana.com/docs/loki/latest/send-data/promtail/)
- [Prometheus](https://grafana.com/oss/prometheus/)