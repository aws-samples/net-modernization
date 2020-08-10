## The Unishop Basket Lambda
This directory contains the source code and unit tests for the basket Lambda function. The basket service gets strangled out from the legacy monolith and converted to a Lambda function that interacts with DynamoDB.

### Build
Contents in this folder are for internal use. Workshop participants can ignore it.

### Src
The Src folder contains the source code for the basket Lambda function. The Lambda function is designed to accept an [APIGatewayEvent](https://github.com/aws/aws-lambda-dotnet/tree/master/Libraries/src/Amazon.Lambda.APIGatewayEvents) and interacts with DynamoDB using the [.NET: Object Persistence Model](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html).

### Test
The test folder contains the unit tests for the the Lambda function, DynamoDB classes.
