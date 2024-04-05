# docker-commands

```
$ docker system prune -a Remove all unused containers, networks, images (both dangling and unreferenced), and optionally, volumes.
$ docker build --no-cache -t=app/dash:latest .
$ docker run -p 49162:80 -d <image label>:latest
$ docker exec -it <container id> /bin/bash
$ docker stop <container id>
$ docker logs --follow <container id>
$ docker run -v d:/data:/data <image_name>
$ docker run -v %cd%/data:/data <image_name> #windows current directory path
$ docker run --rm -it -v ${PWD}:/usr/src/project gcc:4.9 # powershell current path
$ docker stats <container id> --all --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}"

$ docker ps -a
$ docker run -i -t -v /var/app/current:/usr/src/app/ <image_name> /bin/bash
$ docker run -i -t -e FOO=foo -e BAR=bar <image_name> /bin/bash

$docker run -v c:/temp/ssl:/work -it frapsoft/openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout privateKey.key -out /work/certificate.crt  ### generate SSL certificate


#Linux
#Containers
$ docker rm $(docker ps -a -q)

#Images
$ docker rmi $(docker images -q)

#Windows
#Containers
$ FOR /f "tokens=*" %i IN ('docker ps -a -q') DO docker rm %i

#Images
$ FOR /f "tokens=*" %i IN ('docker images -q -f "dangling=true"') DO docker rmi %i


Run node environment
$ docker run --entrypoint /bin/bash -p 49165:3000 -v $(pwd):/usr/src/app -w /usr/src/app  -i -t node:10

Run curl in container
$ docker run -i -t ellerbrock/alpine-bash-curl-ssl /bin/bash
```

### Save/load images to/from filesystem
```
docker save -o c:/myfile.tar centos:16
docker load -i c:/myfile.tar
```

### Connect containers by container name
```
$ docker network create --driver bridge sw-net                     # craete network with name "sw-net"
$ docker run -p 8433:80 --network=sw-net --name sw-web docker-web  # connect container to network "sw-net" setup container name "sw-web"
$ docker network inspect sw-net                                    # validate if containers in network       
$ docker run -i -t ellerbrock/alpine-bash-curl-ssl /bin/bash       # send request from container by name
$ curl http://sw-web/api

$ docker network connect <network_name> <containerid>
```
### Docker build
--progress plain: log all build steps
--no-cache: build without cache
```
docker build --build-arg VERSION=1.0.0 -f ./Dockerfile -t local:test --no-cache --progress plain .
```

### Optimize the docker image build layers cache
#### Show cache by layers
```bash
docker buildx du --verbose
```
#### Delete only if build cache more then 30gb
```
docker buildx prune --filter=type=source.local --force --keep-storage=30gb
```

# Docker Http Client
* [Docker Http Client Test](https://github.com/khdevnet/docker-commands/blob/master/DockerClient.Tests/README.md)
