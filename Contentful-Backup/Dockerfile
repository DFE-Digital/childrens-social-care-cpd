FROM node
RUN npm install -g contentful-cli && mkdir -p /opt/backup
WORKDIR /opt/backup
RUN useradd -m nonroot && chown -R nonroot /opt/backup
USER nonroot
ENTRYPOINT ["sh", "-c", "contentful space export --management-token $token --space-id=$space --environment-id=$envid"]