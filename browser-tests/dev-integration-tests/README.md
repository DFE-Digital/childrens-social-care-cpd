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
Build the docker image first with:
```
docker build -t your-image-name-here .
```
Note the full stop at the end is part of the command.

Then you can run the dockerized tests against your localhost instance using a command like:
```
docker run -it --rm --ipc=host -v //c/test-results:/app/test-results --env PLAYWRIGHT_BASE_URL=http://host.docker.internal:5112 --env CI=1 --workdir /app cpd-playwright npx playwright test
```

Let's breakdown the command:

| Flag        | Value                                                | Info |
| ----------- | -----------                                          | ----------- |
| -it         |                                                      | Allocates a pseudo-TTY connected to the container’s stdin; creating an interactive bash shell in the container|
| --rm        |                                                      | Remove the container once it exits |
| --ipc       | host                                                 | Use the host's IPC profile         |
| -v          | //c/test-results:/app/test-results                   | Maps a local path to container path, allows for easy access to the test results |
| --env       | PLAYWRIGHT_BASE_URL=http://host.docker.internal:5112 | Sets an environment variable in the container, used by the playwright config |
| --env       | CI=1                                                 | Sets an environment variable in the container, makes playwright run in CI mode |
| --workdir   | /app                                                 | Sets the working directory for the container process |
|             | npx playwright test                                  | The cmd line run in the container |