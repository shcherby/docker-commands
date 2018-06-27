# docker-commands

```
$ docker build --no-cache -t u12_core -f u12_core .
$ docker run -p 49162:80 -d app/dash:latest

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
