# Solution-Template
A template for building .NET microservices with best practices and common patterns.

## Initialize Application
Follow these steps to initialize the application on your local machine.
### On macOS/Linux
First, make sure the `init-app.sh` script has executable permissions
```
chmod +x init-app.sh
```
Then build the application using
```
./init-app.sh build
```
Start the application with
```
./init-app.sh start
```
Alternatively, run the application in watch mode
```
./init-app.sh watch
```
### On Windows
Make sure the init-app.cmd script is in the project root then from PowerShell or Command Prompt run
```
.\init-app.cmd build
```
Start the application
```
.\init-app.cmd start
```
Alternatively run the application in watch mode
```
.\init-app.cmd watch
```

### On Docker
Use the following command to start the application in Docker container.
```
docker-compose --env-file src/Microservice/.env.docker -f ./src/Microservice/docker-compose.yml up --build -d
```
Once the containers are up, access the Swagger UI at [http://localhost:8080/swagger](http://localhost:8080/swagger)

## Render Deployment
Render may be in sleep mode on the first request. You may need to wait for a few seconds. [Live Url](https://api-0oqs.onrender.com/swagger)

## Monitoring Tool
Enable real‑time metrics, logs, and health checks for the application by following the step‑by‑step setup guide. [Click for Guide](dockerfiles/monitoring/GUIDE.md)
