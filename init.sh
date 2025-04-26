#!/usr/bin/env bash
set -e

PROJECT="src/Microservice/Microservice.csproj"
CMD=$1

case "$CMD" in
  build) dotnet build $PROJECT --configuration Development ;;
  start) dotnet run --project $PROJECT --environment Development ;;
  watch) dotnet watch run --project $PROJECT --environment Development ;;
  *) echo "Usage: $0 {build|start|watch}" && exit 1 ;;
esac