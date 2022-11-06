## Run container in azure container instance
[aci-integration](https://docs.docker.com/cloud/aci-integration/)
```
az account list-locations -o table
az group create -l westeurope -n antondoctests
docker login azure
docker context create aci myacicontext  --subscription-id 446247d4-0807-4bc1-9f14 --resource-group antondoctests --location westeurope
docker context use myacicontext
docker run -p 80:80 nginx
az group delete -n antondoctests -y --no-wait
```


## Run custom docker container in AppService
[container-linux](https://learn.microsoft.com/en-us/azure/app-service/tutorial-custom-container?pivots=container-linux)
```
```
