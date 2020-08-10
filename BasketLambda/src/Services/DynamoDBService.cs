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
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BasketLambda.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BasketLambda
{
    public class DynamoDBService : IDynamoDBService
    {
        private readonly IDynamoDBRepository dynamoDBRepository;

        public DynamoDBService(IDynamoDBRepository dynamoDB)
        {
            this.dynamoDBRepository = dynamoDB;
        }

        /// <inheritdoc/>
        public async Task<string> GetBasket(string userId)
        {
            Basket fetchedBasket;
            if ((fetchedBasket = await this.dynamoDBRepository.GetDocument(userId)) == null)
            {
                return JsonConvert.SerializeObject(fetchedBasket);
            }
            else
            {
                var baskets = (from item in fetchedBasket.Items
                               select new
                               {
                                   user_id = userId,
                                   item.basket_id,
                                   item.unicorn_id,
                               }).ToList();

                return JsonConvert.SerializeObject(baskets);
            }
        }

        /// <inheritdoc/>
        public async Task<string> DeleteBasket(string userId, string basketId)
        {
            Basket fetchedBasket;
            if ((fetchedBasket = await this.dynamoDBRepository.GetDocument(userId)) != null)
            {
                fetchedBasket.Items.RemoveAll(item => item.basket_id == basketId);
                await this.dynamoDBRepository.SaveDocument(fetchedBasket);
                return JsonConvert.SerializeObject(fetchedBasket);
            }
            else
            {
                return JsonConvert.SerializeObject(fetchedBasket);
            }
        }

        /// <inheritdoc/>
        public async Task<string> AddToBasket(string userId, string unicornId)
        {
            Item item = new Item
            {
                basket_id = Guid.NewGuid().ToString(),
                unicorn_id = unicornId,
            };

            Basket fetchedBasket;

            if ((fetchedBasket = await this.dynamoDBRepository.GetDocument(userId)) != null)
            {
                fetchedBasket.Items.Add(item);
            }
            else
            {
                fetchedBasket = new Basket
                {
                    user_id = userId,
                };
                fetchedBasket.Items = new List<Item>
                {
                    item,
                };
            }

            await this.dynamoDBRepository.SaveDocument(fetchedBasket);

            return JsonConvert.SerializeObject(fetchedBasket);
        }
    }
}
