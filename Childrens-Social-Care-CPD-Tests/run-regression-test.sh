#!/bin/sh -e

docker-compose build

docker-compose up --exit-code-from children-social-care-cpd-regression-suite

docker-compose down --volumes