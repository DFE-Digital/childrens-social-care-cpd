FROM node
RUN npm install -g contentful-cli && mkdir -p /opt/backup
WORKDIR /opt/backup
ENTRYPOINT ["sh", "-c", "contentful space export --management-token $token --space-id=$space --environment-id=$envid"]