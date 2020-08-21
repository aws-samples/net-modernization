# AWS CloudFormation Templates

This folder contains the CloudFormation templates that will provision resources you need in the workshop.

## Workshop-CFN.yml
   This is a top-level template. It refers to another three templates, which are stored in a S3 bucket owned by AWS. When you run this template in CloudFormation, it will run templates it refers to and create all the resources for the workshop. 
   
   You need to run this template in CloudFormation. Please refer to [this tutorial](https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/cfn-using-console.html) for more detailed instructions.

   **NOTE: This template is intended to be run on empty accounts. As a result, it creates a dms-vpc-role and a dms-cloudwatch-logs-role. If either of these roles exist in your account prior to deploying the above template, please delete them from your account. Otherwise, the CloudFormation deployment will fail.**

## Modernization-Resources-CFN.yml
   This template defines most resources you need for the workshop. It is one of templates referred by the Workshop-CFN.yml. This template cannot be run seperately, since it is dependent on other templates which are not provided in this repository. To provision resources you need for the workshop, please build up a CloudFormation template using Workshop-CFN.yml.

## Dev folder
   This folder is for internal use. Workshop participants can ignore it.

