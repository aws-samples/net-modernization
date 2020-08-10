# Locust Test Code

[Locust](https://locust.io/) is an open-source load testing tool . It allows you to write tests for your web application and helps you to find bottlenecks or other performance issues. This folder contains a locust test you will use in the workshop.

## Src

The locustfile.py contains the locust test. This file defines two tests which test /api/unicorn endpoint and /api/unicorn/{id} endpoint with GET method.

In the workshop, the locust test will be containerized and deployed to AWS Elastic Beanstalk. The Dockerfile and Dockerrun.aws.json are for this purpose. The Dockerfile describes how Docker should build the image for the locust test, while the Dockerrun.aws.json describes how AWS Elastic Beanstalk should configure the locust test in a multicontainer Docker environment.

## Build
Contents in this folder are for internal use. Workshop participants can ignore them.


  


