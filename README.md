## .NET Modernization - Unicorn Workshop

In this workshop, you'll learn to move a monolithic .NET application to a microservices-based .NET Core appliction by applying the [Strangler Fig Pattern](https://martinfowler.com/bliki/StranglerFigApplication.html). After completing the workshop, you will have the skills to feel more conmfortable modernzing your legacy .NET Framework applications using AWS services and features such as [Amazon RDS](https://aws.amazon.com/rds/), [Amazon API Gateway](https://aws.amazon.com/api-gateway/), [Amazon Cognito](https://aws.amazon.com/cognito/), [AWS Lambda](https://aws.amazon.com/lambda/), [Amazon Simple Storage Service](https://aws.amazon.com/s3/), [Amazon DynamoDB](https://aws.amazon.com/dynamodb/), [AWS Schema Conversion Tool](https://aws.amazon.com/dms/schema-conversion-tool/), [AWS Data Migration Service](https://aws.amazon.com/dms/), [Amazon Elastic Container Service](https://aws.amazon.com/ecs/), and [Amazon Aurora](https://aws.amazon.com/rds/aurora/). 

### Background
Unishop is THE one-stop-shop for all your Unicorn needs. You can find the best Unicorn selection online at the Unishop and get your Unicorn delivered in less than 24 hours!

As a young startup Unishop built a great service which was focused on customers and business outcomes but less on technology and architecture. After a few years establishing a business model and securing the next round of venture capital funding, the business is looking to expand to other markets, such as Unicorn-Insurance, Unicorn-Banking and Unicorn-Ride-Sharing. The CEO asked the CTO to prepare the technology stack and start re-architecting Unishop solution to ensure that the right foundations are in place for supporting the business plan.

As part of this workshop the CTO would like to explore moving to a microservices-based architecture using the Strangler Fig pattern and modernizing Unishop's legacy application.


### Labs

The instructions for each lab in the workshop can be found here: https://net-modernization.workshop.aws/.

| Lab | Description |
| ------ | ------ |
| [Lab 1: Environment Setup](https://net-modernization.workshop.aws/lab1.html) | Set up pre-configured AWS resources, connect to the remote developer environment, download the code for the workshop, and view the legacy application |
| [Lab 2: Facade Configuration](https://net-modernization.workshop.aws/lab2.html) | Front the legacy application with API Gateway and enable authentication with Amazon Cognito   |
| [Lab 3: Serverless Basket Service](https://net-modernization.workshop.aws/lab3.html) | Use AWS Lambda and DynamoDB to strangle out the basket microservice|
| [Lab 4: Database Migration](https://net-modernization.workshop.aws/lab4.html) | Migrate inventory service's Microsoft SQL Server data to Amazon Aurora |
| [Lab 5: Containerized Inventory Service](https://net-modernization.workshop.aws/lab5.html) | Extract and deploy the inventory microservice using Amazon Elastic Container Service |
| [Extra Credit](https://net-modernization.workshop.aws/extra.html) | Utilize [Amazon Elastic Kubernetes Services](https://aws.amazon.com/eks/) and [Amazon Rekognition](https://aws.amazon.com/rekognition/) to further modernize the Unishop |

### Repository Structure

The UnicornWorkshop.sln file in the root directory contains the structure for the entire workshop. It opens up the five projects that will be used during the workshop.
The CFN directory stores CloudFormation templates that you can run to set up/pre-configure all the necessary Amazon resources for the workshop. 
The MonolithicApplication directory contains the source code for the legacy application. 
The FrontEnd directory contains the source code for the front-end of the Unishop which is statically hosted in the microservices architecture. 
The BasketLambda directory has the source code used to move the basket service from the monolith to AWS Lambda. 
The Locust directory contains the source code and files necessary to use the [Locust](https://locust.io/) load testing tool. 
The InventoryService directory contains the source code used for the containerization of the Unishop inventory service. 

### Launch Your Own Workshop
We prepared AWS accounts for you to use, but you can also go through this workshop in your own AWS account. If you want to do this, please download [this CloudFormation template](https://unicorn-store-dotnet.s3.us-east-2.amazonaws.com/Workshop-CFN.yml) and deploy it in your own AWS account. The workshop is currently only supported in the following regions: us-east-1, us-west-2, eu-west-1, and ap-southeast-1. You can also launch this CloudFormation stack, using your account, with the links below:

| AWS Region Code            | Name                     | Launch |
| --- | --- | --- 
| us-east-1 |US East (N. Virginia)|[Launch Stack](https://console.aws.amazon.com/cloudformation/home?region=us-east-1#/stacks/new?stackName=UnicornWorkshopMain&templateURL=https://unicorn-store-dotnet.s3.us-east-2.amazonaws.com/Workshop-CFN.yml) |
| us-west-2 |US West (Oregon)| [Launch Stack](https://console.aws.amazon.com/cloudformation/home?region=us-west-2#/stacks/new?stackName=UnicornWorkshopMain&templateURL=https://unicorn-store-dotnet.s3.us-east-2.amazonaws.com/Workshop-CFN.yml) |
| eu-west-1 |EU (Ireland)| [Launch Stack](https://console.aws.amazon.com/cloudformation/home?region=eu-west-1#/stacks/new?stackName=UnicornWorkshopMain&templateURL=https://unicorn-store-dotnet.s3.us-east-2.amazonaws.com/Workshop-CFN.yml) |
| ap-southeast-1 |AP (Singapore)| [Launch Stack](https://console.aws.amazon.com/cloudformation/home?region=ap-southeast-1#/stacks/new?stackName=UnicornWorkshopMain&templateURL=https://unicorn-store-dotnet.s3.us-east-2.amazonaws.com/Workshop-CFN.yml) |

### Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

### License

This library is licensed under the MIT-0 License. See the LICENSE file.
