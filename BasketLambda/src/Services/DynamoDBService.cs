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
using System.Threading.Tasks;
using BasketLambda.Interfaces;
using Newtonsoft.Json;

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
            var basketItems = await this.dynamoDBRepository.GetUserDocuments(userId);
            return JsonConvert.SerializeObject(basketItems);
        }

        /// <inheritdoc/>
        public async Task<string> DeleteBasket(string userId, string basketId)
        {
            var fetchedBasket = await this.dynamoDBRepository.GetDocument(userId, basketId);
            if (fetchedBasket != null)
            {
                await this.dynamoDBRepository.DeleteDocument(userId, basketId);
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
            var newBasketItem = new Basket
            {
                basket_id = Guid.NewGuid().ToString(),
                unicorn_id = unicornId,
                user_id = userId,
            };

            await this.dynamoDBRepository.SaveDocument(newBasketItem);

            return JsonConvert.SerializeObject(newBasketItem);
        }

        /// <inheritdoc/>
        public async Task UpdateItemAvailability(string unicornId, bool available)
        {
            var basketItems = await this.dynamoDBRepository.GetUnicornDocuments(unicornId);
            foreach (var item in basketItems)
            {
                item.available = available;
                await this.dynamoDBRepository.SaveDocument(item);
            }
        }
    }
}
