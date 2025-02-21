# Use an outdated base image with known vulnerabilities
FROM ubuntu:16.04

# Install a vulnerable version of Node.js
RUN apt-get update && apt-get install -y nodejs=6.9.1 npm

# Expose ports in a way that makes the container insecure
EXPOSE 80
EXPOSE 443

# Run the app as the root user (highly insecure)
USER root

# Add a sample application code (just an example)
COPY . /app

# Install dependencies with an outdated vulnerable version of Express
WORKDIR /app
RUN npm install express@4.16.0

# Start the application (insecure entry point)
CMD ["node", "app.js"]
