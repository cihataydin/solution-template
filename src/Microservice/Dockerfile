FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-restore --verbosity normal
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Microservice.dll", "--urls", "http://+:80"]