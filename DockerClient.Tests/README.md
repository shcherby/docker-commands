# Docker Http Client Samples

* [Docker DotNet Http Client](https://github.com/dotnet/Docker.DotNet)
* Docker API version 1.4 # docker version

### Create container and wait before it fully initialized, container logs use for this 

```
        [Fact]
        public async void CreateContainerAndWaitBeforeItStartedTest()
        {
            var mongoDbImageName = "mongo";
            var mongoDbImageTag = "4.0";
            var mongodDbImage = $"{mongoDbImageName}:{mongoDbImageTag}";
            var mongoDbContainerName = "mongo-tests";
            var exposedPort = $"27017/tcp";
            DockerClient client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
                 .CreateClient();

            await CreateImageIfNotExist(client, mongoDbImageName, mongoDbImageTag);
            await RemoveContainerIfExist(client, "mongo-tests");

            // docker run --name mongo-tests -p 33381:27017 -d mongo:4;
            var containerId = await RunContainer(client, mongodDbImage, mongoDbContainerName, exposedPort, "33381");

            await WaitBeforeContainerInit(client, containerId, "waiting for connections on port 27017");

            ContainerListResponse container = await GetContainerByName(client, mongoDbContainerName);

            Assert.NotNull(container);

            await RemoveContainerIfExist(client, "mongo-tests");

            client.Dispose();
        }
```
