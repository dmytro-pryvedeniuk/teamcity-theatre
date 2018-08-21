FROM microsoft/aspnetcore-build AS builder
WORKDIR /app

# install yarn
RUN apt-get update && apt-get install -y curl apt-transport-https && \
    curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | apt-key add - && \
    echo "deb https://dl.yarnpkg.com/debian/ stable main" | tee /etc/apt/sources.list.d/yarn.list && \
    apt-get update && apt-get install -y yarn

# Copy csproj/sln files first to reuse Docker cache
COPY src/TeamCityTheatre.Web/*.csproj ./src/TeamCityTheatre.Web/
COPY src/TeamCityTheatre.Core/*.csproj ./src/TeamCityTheatre.Core/
COPY src/TeamCityTheatre.sln ./src
RUN dotnet restore ./src/TeamCityTheatre.sln

# Install yarn dependencies to reuse Docker cache
COPY src/TeamCityTheatre.Web/package.json src/TeamCityTheatre.Web/yarn.lock ./src/TeamCityTheatre.Web/
RUN cd ./src/TeamCityTheatre.Web \
    && yarn --cwd ./src/TeamCityTheatre.Web install --pure-lockfile

# Copy all files and call publish script (we utilize Docker cache above so it should be faster after the first run)
COPY . .

RUN cd ./src/TeamCityTheatre.Web \
    && yarn run build:release \
    && dotnet publish TeamCityTheatre.Web.csproj --configuration Release --verbosity normal --output /app/publish-output

# Build runtime image
FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=builder /app/publish-output/ .
ENTRYPOINT ["dotnet", "TeamCityTheatre.Web.dll"]