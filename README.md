# solution-template
A template for building .NET microservices with best practices and common patterns.

## Contents
- [Initialize Application](#initialize-application)
    - [On macOS/Linux](#on-macoslinux)
    - [On Windows](#on-windows)
    - [On Docker](#on-docker)
- [API Interfaces](#api-interfaces)
    - [Swagger UI](#swagger-ui)
    - [Scalar UI](#scalar-ui)
- [Monitoring Tools](#monitoring-tools)
- [Deployment](#deployment)
- [Technologies and References](#technologies-and-references)

## Initialize Application
Follow these steps to initialize the application on your local machine.
### On macOS/Linux
First, make sure the `init.sh` script has executable permissions
```
chmod +x init.sh
```
Then build the application using
```
./init.sh build
```
Start the application with
```
./init.sh start
```
Alternatively, run the application in watch mode
```
./init.sh watch
```
### On Windows
From PowerShell or Command Prompt run build command
```
.\init.cmd build
```
Start the application
```
.\init.cmd start
```
Alternatively run the application in watch mode
```
.\init.cmd watch
```

### On Docker
Use the following command to start the application in Docker container.
```
docker-compose --env-file .env.docker up --build -d
```

## API Interfaces
### Swagger UI
- Local host url: [http://localhost:8000/swagger](http://localhost:8000/swagger)
- Docker host url: [http://localhost:8080/swagger](http://localhost:8080/swagger)
### Scalar UI
- Local host url: [http://localhost:8000/scalar](http://localhost:8000/scalar)
- Docker host url: [http://localhost:8080/scalar](http://localhost:8080/scalar)

## Monitoring Tools
Enable real‑time metrics, logs, and health checks for the application by following the step‑by‑step setup guide. [Click for Guide](dockerfiles/monitoring/GUIDE.md)

## Deployment
Service may be in sleep mode on the first request. You may need to wait for a few seconds. [Live Demo Url](https://api-0oqs.onrender.com/scalar)

## Technologies and References
- [.NET](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Render](https://render.com/)
- [Scalar](https://scalar.com/)
- [Swagger](https://swagger.io/)

## License
solution-template is licensed under the [MIT](LICENSE) license.
