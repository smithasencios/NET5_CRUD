# Using alpline linux due to the small footprint
# Will switch back to the normal image if alpine becomes too cumbersome
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS development

WORKDIR /app/Api
# "watch" reloads the webserver whenever a source file is edited.
# --no-launch-profile prevents launchSettings.json from overriding env variables
# 0.0.0.0 in the URL allows access to the webserver from outside the Docker image
ENTRYPOINT [ "dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:5000"]