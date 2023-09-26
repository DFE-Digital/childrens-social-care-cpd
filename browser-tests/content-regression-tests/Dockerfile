FROM mcr.microsoft.com/playwright:v1.37.1-jammy

WORKDIR /app

COPY ./tests /app/tests/
COPY ./package*.json /app/
COPY ./playwright.config.ts /app/

RUN npm ci