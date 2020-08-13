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
using Amazon.DynamoDBv2.DataModel;
using BasketLambda.Interfaces;

namespace BasketLambda.Repository
{
    public class DynamoDBRepository : IDynamoDBRepository
    {
        private const string UnicornIdIndexName = "unicorn_id_index";

        private IDynamoDBContext dynamoDBContext;

        public DynamoDBRepository(IDynamoDBContext context)
        {
            this.dynamoDBContext = context;
        }

        /// <inheritdoc/>
        public async Task DeleteDocument(string userId, string basketId)
        {
            await this.dynamoDBContext.DeleteAsync<Basket>(userId, basketId);
        }

        /// <inheritdoc/>
        public async Task<Basket> GetDocument(string userId, string basketId)
        {
            return await this.dynamoDBContext.LoadAsync<Basket>(userId, basketId);
        }

        /// <inheritdoc/>
        public async Task<List<Basket>> GetUnicornDocuments(string unicornId)
        {
            var query = this.dynamoDBContext.QueryAsync<Basket>(unicornId, new DynamoDBOperationConfig
            {
                IndexName = UnicornIdIndexName
            });
            return await query.GetRemainingAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Basket>> GetUserDocuments(string userId)
        {
            var query = this.dynamoDBContext.QueryAsync<Basket>(userId);
            return await query.GetRemainingAsync();
        }

        /// <inheritdoc/>
        public async Task SaveDocument(Basket basket)
        {
            await this.dynamoDBContext.SaveAsync(basket);
        }
    }
}
