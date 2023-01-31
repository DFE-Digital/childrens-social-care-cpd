# Load testing

Load testing will be performed by a github action against the load testing environment, which will be spun up accordingly to automate the process.

The load test will be performed by [locust](https://locust.io) with a python script to test user journeys. The script is found in this directory and will be run by a github action called `load-testing.yml`