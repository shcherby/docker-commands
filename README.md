# docker-commands

```
$ docker build --no-cache -t u12_core -f u12_core .
$ docker run -p 49162:80 -d <image label>:latest
$ docker exec -it <container id> /bin/bash
$ docker stop <container id>
$ docker logs --follow <container id>
$ docker run -v d:/data:/data <image_name>

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

```
