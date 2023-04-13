# Dockerfile for Contentful Space Export
This Dockerfile creates a Docker image that exports a Contentful space using the Contentful CLI. It installs the CLI, sets the working directory, and defines the entrypoint to execute the space export command.

## Dockerfile
```
FROM node
RUN npm install -g contentful-cli && mkdir -p /opt/backup
WORKDIR /opt/backup
ENTRYPOINT ["sh", "-c", "contentful space export --management-token $token --space-id=$space --environment-id=$envid"]
```

## Image description
1. **FROM node**: This line specifies the base image as the official Node.js Docker image.
2. **RUN npm install -g contentful-cli && mkdir -p /opt/backup**: Installs the Contentful CLI globally and creates a backup directory at /opt/backup.
3. **WORKDIR /opt/backup**: Sets the working directory of the Docker image to /opt/backup.
4. **ENTRYPOINT**: Specifies the default command to run when the container starts. In this case, it runs the contentful space export command using the provided environment variables for management-token, space-id, and environment-id.

## Usage
To build the Docker image, run the following command in the same directory as the Dockerfile:

```
docker build -t contentful-space-export .
```

To run the container and export a Contentful space, execute the following command with the required environment variables:

```
docker run --rm -e token=MANAGEMENT_TOKEN -e space=SPACE_ID -e envid=ENVIRONMENT_ID -v /local/path/to/backup:/opt/backup contentful-space-export
```

Replace `MANAGEMENT_TOKEN`, `SPACE_ID`, `ENVIRONMENT_ID`, and `/local/path/to/backup` with your actual Contentful Management API token, space ID, environment ID, and the local path where you want to save the backup, respectively.

This command will run the container, export the specified Contentful space, and save the backup to the specified local path. The container will be removed after the export process is completed.
