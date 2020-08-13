/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using BasketLambda.Interfaces;
using BasketLambda.Repository;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BasketLambda
{
    public class Function
    {
        private IDynamoDBRepository dynamoDBRepository;

        private IDynamoDBService dynamoDBService;

        public Function(IDynamoDBService service)
        {
            this.dynamoDBService = service;
        }

        public Function()
        {
            var client = new AmazonDynamoDBClient();
            this.dynamoDBRepository = new DynamoDBRepository(new DynamoDBContext(client));
            this.dynamoDBService = new DynamoDBService(this.dynamoDBRepository);
        }

        /// <summary>
        /// Function to carry out Basket POST,GET,DELETE operations depending on Http method.
        /// </summary>
        /// <param name="apigProxyEvent"></param>
        /// <param name="context"></param>
        /// <returns> APIGatewayProxyResponse with body as basket object </returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            string body = string.Empty;

            var headers = new Dictionary<string, string>
            {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Allow", "GET, OPTIONS, POST, DELETE" },
                    { "Access-Control-Allow-Methods", "GET, OPTIONS, POST, DELETE" },
                    { "Access-Control-Allow-Headers", "*" },
            };

            try
            {
                var claims = apigProxyEvent.RequestContext.Authorizer.Claims;

                switch (apigProxyEvent.HttpMethod)
                {
                    case "GET":
                        body = await this.dynamoDBService.GetBasket(apigProxyEvent.PathParameters["id"]);
                        break;
                    case "DELETE":
                        body = await this.dynamoDBService.DeleteBasket(claims["sub"], apigProxyEvent.PathParameters["id"]);
                        break;
                    case "POST":
                        var basketRequest = JsonConvert.DeserializeObject<BasketRequestBody>(apigProxyEvent.Body);
                        body = await this.dynamoDBService.AddToBasket(claims["sub"], basketRequest.unicorn_id);
                        break;
                    default:
                        break;
                }

                return new APIGatewayProxyResponse
                {
                    Body = body,
                    Headers = headers,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                return new APIGatewayProxyResponse
                {
                    Body = e.Message,
                    Headers = headers,
                    StatusCode = 500,
                };
            }
        }
        
        /// <summary>
        /// Function to carry out Basket item status update on SNS event.
        /// </summary>
        /// <param name="snsEvent"></param>
        public async Task SNSHandler(SNSEvent snsEvent)
        {
            foreach (var record in snsEvent.Records)
            {
                var snsRecord = record.Sns;
                var unicornAvailableMessage = JsonConvert.DeserializeObject<SNSMessage>(snsRecord.Message);
                await this.dynamoDBService.UpdateItemAvailability(unicornAvailableMessage.unicorn_id, unicornAvailableMessage.available);
            }
        }
    }
}
