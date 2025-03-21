# Playwright Tests 

All tests can be found under the `tests/` folder.

## For Developers
Set up a local environment variable called `PLAYWRIGHT_BASE_URL` with the value being the root url for the site to test against.  For example, testing against your locally running dev website you might execute:
```
export PLAYWRIGHT_BASE_URL=https://localhost:7112/
```
The integration tests have to be running against a contentful environment running in preview mode.  Make sure your local config has `CPD_CONTENTFUL_FORCE_PREVIEW` set to `true`

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
