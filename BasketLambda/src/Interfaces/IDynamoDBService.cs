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
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace BasketLambda.Interfaces
{
    public interface IDynamoDBService
    {
        /// <summary>
        /// Function to fetch user basket based on user id.
        /// </summary>
        /// <param name="userId">User id. </param>
        /// <returns>Serialized List of basket objects. </returns>
        public Task<string> GetBasket(string userId);

        /// <summary>
        /// Function to delete a item from user basket based on basket id.
        /// </summary>
        /// <param name="userId">User Id to get basket. </param>
        /// <param name="basketId">Basket item to delete. </param>
        /// <returns>Serialized Modified user basket. </returns>
        public Task<string> DeleteBasket(string userId, string basketId);

        /// <summary>
        /// Function to add a new item to user basket.
        /// </summary>
        /// <param name="userId">User Id to add to basket. </param>
        /// <param name="unicornId">Unicorn Id to add to user basket. </param>
        /// <returns>Serialized Updated user basket. </returns>
        public Task<string> AddToBasket(string userId, string unicornId);
    }
}
