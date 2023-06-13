#!/bin/bash -e
docker-compose up --exit-code-from e2e

docker-compose down --volumes