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

using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using BasketLambda.Interfaces;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BasketLambda.Tests
{
    public class FunctionTest
    {
        private Mock<IDynamoDBService> utility;
        private APIGatewayProxyRequest request;
        private TestLambdaContext context;
        private Function function;

        [Fact]
        public void FunctionHandler_GET_Return200()
        {
            this.GivenRequestWith("GET", string.Empty);
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(200, "because GET switch case only contains mocked function call");
        }

        [Fact]
        public void FunctionHandler_POSTValidJsonBody_Return200()
        {
            this.GivenRequestWith("POST", JsonConvert.SerializeObject(new BasketRequestBody()));
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(200, "because this posts a valid basket request body");
        }

        [Fact]
        public void FunctionHandler_POSTInvalidJsonBody_Return500()
        {
            this.GivenRequestWith("POST", string.Empty);
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(500, "because this posts an invalid basket request body");
        }

        [Fact]
        public void FunctionHandler_DELETE_Return200()
        {
            this.GivenRequestWith("DELETE", string.Empty);
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(200, "because DELETE switch case only contains mocked function call");
        }

        [Fact]
        public void FunctionHandler_Other_Return200()
        {
            this.GivenRequestWith(string.Empty, string.Empty);
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(200, "because the default switch case is empty");
        }

        [Fact]
        public void FunctionHandler_EmptyRequest_Return500()
        {
            this.GivenEmptyRequest();
            this.GivenLambdaContext();
            this.GivenDDBUtility();
            this.GivenFunctionHandler();

            var actualResponse = this.WhenLambdaFunctionTriggered();

            actualResponse.StatusCode.Should().Be(500, "because the request is empty/invalid");
        }

        private void GivenRequestWith(string method, string body)
        {
            this.request = new APIGatewayProxyRequest();
            this.request.RequestContext = new APIGatewayProxyRequest.ProxyRequestContext();
            this.request.RequestContext.Authorizer = new APIGatewayCustomAuthorizerContext();
            this.request.RequestContext.Authorizer.Claims = new Dictionary<string, string>();
            this.request.RequestContext.Authorizer.Claims.Add("sub", string.Empty);

            this.request.PathParameters = new Dictionary<string, string>();
            this.request.PathParameters.Add("id", string.Empty);

            this.request.Body = body;
            this.request.HttpMethod = method;
        }

        private void GivenEmptyRequest()
        {
            this.request = new APIGatewayProxyRequest();
        }

        private void GivenDDBUtility()
        {
            this.utility = new Mock<IDynamoDBService>();
            this.utility.Setup(x => x.GetBasket(It.IsAny<string>())).Returns((string id) => Task.FromResult(string.Empty));
            this.utility.Setup(x => x.DeleteBasket(It.IsAny<string>(), It.IsAny<string>())).Returns((string user_id, string basket_id) => Task.FromResult(string.Empty));
            this.utility.Setup(x => x.AddToBasket(It.IsAny<string>(), It.IsAny<string>())).Returns((string user_id, string unicorn_id) => Task.FromResult(string.Empty));
        }

        private void GivenLambdaContext()
        {
            this.context = new TestLambdaContext();
        }

        private void GivenFunctionHandler()
        {
            this.function = new Function(this.utility.Object);
        }

        private APIGatewayProxyResponse WhenLambdaFunctionTriggered()
        {
            return this.function.FunctionHandler(this.request, this.context).GetAwaiter().GetResult();
        }
    }
}
