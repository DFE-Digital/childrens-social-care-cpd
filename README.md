# childrens-social-care-cpd

This repository contains the code needed to host the Social Workforce career progression service. This service helps workers in this profession to easily find information about the pathways to further their career.

## Live examples

The live site can be found [here](https://www.support-for-social-workers.education.gov.uk)

## Nomenclature

CPD - Career & Professional Development

## Technical documentation

The application is designed to run in a docker container and is dependant on Contentful CMS for content update internally by our content designers.

### Before running the app (if applicable)

You will need a Contentful CMS instance with the correct Data models. The CMS is used in headless mode and the environments can be configured to access preview or delivery endpoints.

The following environment variables are to be configured

| Variable                             | description                                        |
| ------------------------------------ | -------------------------------------------------- |
| CPD_GOOGLEANALYTICSTAG               | The google analytics API (optional)                |
| CPD_CONTENTFUL_ENVIRONMENT           | The contentful environment being consumed          |
| CPD_SPACE_ID                         | The contentful space id                            |
| CPD_DELIVERY_KEY                     | The live content for the environment set           |
| CPD_PREVIEW_KEY                      | The preview content for the environment set        |
| CPD_AZURE_ENVIRONMENT                | The environment for the system                     |
| CPD_INSTRUMENTATION_CONNECTIONSTRING | Application insights connection string             |
| CPD_FEATURE_POLLING_INTERVAL         | Polling interval for feature configuration udpates |
| CPD_SEARCH_CLIENT_API_KEY            | Client API Key for the search service              |
| CPD_SEARCH_ENDPOINT                  | Search service endpoint                            |
| CPD_SEARCH_INDEX_NAME                | The Index name to query against                    |

In order to run the application locally, you can either open the solution, build and run in the IDE of your choice or there is a [docker-compose](~/docker-compose.yml) file that allows for local running of the app.

In order to run the app with the compose file you will need `docker` and `docker-compose` installed and run the following command in the root directory.

`docker-compose up`

### Running the test suite

All pipelines are run in the github actions within this repository, however the service is written in dotnet core and therefore the unit tests can be run as follows:

`dotnet restore`

followed by

`dotnet test`

Cypress testing is also included.

### Compiling the Sass

The Sass is compiled using the `compile-sass.ps1` script in the main project. The commands can also be used in \*nix environments.

## Releases

Information around the Github Release process can be found in the [release documentation](./docs/RELEASE.md).

## Contentful

More information on Contentful can be found [here](https://www.contentful.com/)

---

Copyright (c) 2023 Crown Copyright (Department for Education)
