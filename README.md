# childrens-social-care-cpd

This repository contains the code needed to host the Social Workforce career progression service. This service helps workers in this profession to easily find information about the pathways to further their career

## Live examples

The live site can be found [here](https://www.evelop-child-family-social-work-career.education.gov.uk)

## Nomenclature

CPD - Career & Professional Development

## Technical documentation

The application is designed to run in a docker container and is dependant on Contentful CMS for content update internally by our content designers.

### Before running the app (if applicable)

You will need a Contentful CMS instance with the correct Data models. The CMS is used in headless mode and the environments can be configured to access preview or delivery endpoints.

The following environment variables are to be configured

| Variable | description |
| ---  | --- |
| CPD_GOOGLEANALYTICSTAG | The google analytics API (optional) |
| CPD_CONTENTFUL_ENVIRONMENT | The contentful environment being consumed |
| CPD_SERVICE_ENVIRONMENT | The service environment |
| CPD_DELIVERY_API_KEY | The live content for the environment set |
| CPD_PREVIEW_API_KEY | The preview content for the environment set |

In order to run the application locally, you can either open the solution, build and run in the IDE of your choice or there is a [docker-compose](~/docker-compose.yml) file that allows for local running of the app.

In order to run the app with the compose file you will need `docker` and `docker-compose` installed and run the following command in the root directory.

`docker-compose up` 

### Running the test suite

All pipelines are run in the github actions within this repository, however the service is written in dotnet core and therefore the unit tests can be run as follows:

`dotnet restore`

followed by

`dotnet test`

### Further documentation