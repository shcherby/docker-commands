using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DockerHttpClient.Tests
{
    public class DockerClientTests
    {
        private readonly ITestOutputHelper output;

        public DockerClientTests(ITestOutputHelper output)
        {
            this.output = output;
        }

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

        private async Task WaitBeforeContainerInit(DockerClient client, string containerId, string initKey)
        {
            Stream logsStream = await client.Containers.GetContainerLogsAsync(containerId, new ContainerLogsParameters() { ShowStdout = true, Follow = true });

            using (StreamReader sr = new StreamReader(logsStream))
            {
                string line;
                while ((line = sr.ReadLine()) != null && !line.Contains(initKey))
                {
                    output.WriteLine(line);
                }
            }
        }

        private static async Task<string> RunContainer(DockerClient client, string image, string containerName, string exposedPort, string hostPort)
        {
            CreateContainerResponse container = await CreateContainer(client, image, containerName, exposedPort, hostPort);
            await client.Containers.StartContainerAsync(containerName, new ContainerStartParameters());
            return container.ID;
        }

        private static async Task<CreateContainerResponse> CreateContainer(DockerClient client, string image, string containerName, string exposedPort, string hostPort)
        {
            return await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = image,
                Name = containerName,
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    {exposedPort, new EmptyStruct() }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            exposedPort,
                            new List<PortBinding>
                            {
                                new PortBinding
                                {
                                    HostPort = hostPort
                                }
                            }
                        }
                    },
                }
            });
        }

        private static async Task CreateImageIfNotExist(DockerClient client, string imageName, string tag)
        {
            //reference
            var filters = new Dictionary<string, IDictionary<string, bool>> {
                { "reference", new Dictionary<string, bool>{ { $"{imageName}:{tag}", true } } }
            };
            var images = await client.Images.ListImagesAsync(new ImagesListParameters { Filters = filters });
            if (images.Count == 0)
            {
                await client.Images.CreateImageAsync(
                    new ImagesCreateParameters { FromImage = imageName, Tag = tag }, null,
                    new Progress<JSONMessage>(message =>
                    {
                        Console.WriteLine(!string.IsNullOrEmpty(message.ErrorMessage)
                            ? message.ErrorMessage
                            : $"{message.ID} {message.Status} {message.ProgressMessage}");
                    }));
            }
        }

        private static async Task RemoveContainerIfExist(DockerClient client, string name)
        {
            ContainerListResponse container = await GetContainerByName(client, name);

            if (container != null)
            {
                await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true });

            }
        }

        private static async Task<ContainerListResponse> GetContainerByName(DockerClient client, string name)
        {
            var filters = new Dictionary<string, IDictionary<string, bool>>()
            {
                { "name", new Dictionary<string, bool>() { { name, true } } }
            };

            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters()
            {
                All = true,
                Filters = filters
            });

            return containers.FirstOrDefault();
        }
    }
}
