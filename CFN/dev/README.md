# AWS CloudFormation Templates for Workshop Development Environment

This folder contains CloudFormation templates that future workshop developers can use to set up a development environment.

**NOTE: The templates assume all necessary CodeCommit repositories are already in place at the time of their deployment**

## Workshop-dev-main.yml
   This is the top-level template. It refers to the other three templates, whose copies are also in this folder. When you run this template in CloudFormation, it will run templates it refers to and create all essential resources for the development environment. 

## Dev-S3Buckets.yml
   This template creates the S3 buckets required by CodePipelines for storing CodeBuild artifacts.

## Dev-Resources.yml
   This template creates resources that host the application and its services covered in the workhsop.

## Dev-CodePipeline.yml
   This template creates CodePipelines that builds, tests, and deploys the application and its services based on source code in the CodeCommit repositories.

