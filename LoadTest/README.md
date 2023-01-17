# Load testing

Load testing will be performed by a github action against the load testing environment. Marketplace github action to be implemented with a manual test initially. 

The load test will be performed by [locust](https://locust.io) with a python script to test user journeys. The script is found in this directory and will be run by a github action called `load-testing.yml`

This action will be run manually and at time of writing will be set to test against a load test environment