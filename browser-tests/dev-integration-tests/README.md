# Playwright Tests

All tests can be found under the `tests/` folder.

## For Developers
You can run the tests by running the following:
```
npm install
npx playwright test
```
Note: you may need to install the browsers if you havent done that before. You can do that using `npx playwright install chromium` to install just the one for the tests.

If you want to run with the UI then use:
```
npx playwright test -ui
```

## Docker
The tests are run inside docker for the CI pipeline. To run them you'll need to have built the web app docker image first.

Make sure all the environment variables in the docker-compose file are set in your environment (or passed at the command line), then run:

```
docker-compose up --exit-code-from dev-integration-tests
```