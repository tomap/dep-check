FROM microsoft/dotnet:sdk
WORKDIR /precache

ENV NODE_VERSION 8.14.1
ENV NODE_DOWNLOAD_SHA ae9b04e0ad806dc31242ca02b84a84ea67c978e41f60d94ffca010ed3fe32735

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

COPY . .

RUN dotnet restore

RUN npm install

RUN rm -rf /precache

RUN dotnet tool install dotnet-reportgenerator-globaltool --tool-path /tools

RUN apt-get update
RUN apt-get install -y apt-transport-https gnupg2 software-properties-common
RUN curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add -
RUN add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian $(lsb_release -cs) stable"
RUN apt-get update
RUN apt-get install -y docker-ce docker-ce-cli containerd.io
    
