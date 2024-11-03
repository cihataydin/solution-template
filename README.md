# Solution.Template
A template for building .NET microservices with best practices and common patterns.

## Docker Run Command
`docker-compose --env-file src/Microservice/.env.docker -f ./src/Microservice/docker-compose.yml up --build -d`

## Docker Local Host Url
[Swagger Url](http://localhost:8080/swagger)

## Render Deployment
Render may be in sleep mode on the first request. You may need to wait for a few seconds. [Live Url](https://api-0oqs.onrender.com/swagger)

## Docker Monitoring Tool
[Click for Guide](dockerfiles/monitoring/GUIDE.md)
