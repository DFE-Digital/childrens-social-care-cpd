FROM cypress/included

WORKDIR /app

COPY ./Cypress /app/cypress
COPY ./cypress.config.js /app/cypress.config.js
COPY ./package*.json /app

RUN yarn install