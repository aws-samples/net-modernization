## The Unishop Inventory Service
This directory contains the source code and unit tests for the inventory service. The inventory service gets strangled out from the legacy monolith and containerized using Amazon Elastic Container Service.

### Build
Contents in this folder are for internal use. Workshop participants can ignore it.


### Src
The Src folder contains the source code for the inventory microservice. The code has been organized using the Service-Repository pattern. The source code also includes code to implement the EKS and Rekognition extra credit labs. The file also contains a Docker file used to create the Docker image as well as a appsettings.json file used for configurations.

### Test
The test folder contains the unit tests for the the inventory service controller, repositories, and services.