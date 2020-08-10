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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using BasketLambda;
using BasketLambda.Interfaces;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BasketLambda.Tests
{
    public class DynamoDBServiceTest
    {
        private List<Basket> fakeDynamoDb;
        private Mock<IDynamoDBRepository> dynamoDBRepository;
        private DynamoDBService utility;

        [Fact]
        public void GetBasket_UserFound_ItemsReturned()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var userId = this.GiveUserDocumentWithItem();
            var basketItems = this.WhenGetBasketWithUserId(userId);
            basketItems.Should().NotBeEmpty("because user already exists with items").And.HaveCount(1);
        }

        [Fact]
        public void GetBasket_UserNotFound_NullResult()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var basketItems = this.WhenGetBasketWithUserId(Guid.NewGuid().ToString());
            basketItems.Should().BeNull("because the user does not have a basket");
        }

        [Fact]
        public void DeleteBasket_UserBasketFound_ItemDeleted()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var userId = this.GiveUserDocumentWithItem();
            var basketId = this.fakeDynamoDb[0].Items[0].basket_id;
            var basket = this.WhenDeleteUserBasketItem(userId, basketId);
            basket.Items.Should().HaveCount(0, "because the item was deleted from user basket");
        }

        [Fact]
        public void DeleteBasket_UserBasketFound_InvalidItem()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var userId = this.GiveUserDocumentWithItem();
            var basketId = Guid.NewGuid().ToString();
            var basket = this.WhenDeleteUserBasketItem(userId, basketId);
            basket.Items.Should().HaveCount(1, " because the item was not deleted from user basket");
        }

        [Fact]
        public void AddToBasket_UserFound_ItemAdded()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var userId = this.GiveUserDocumentWithItem();
            var unicornId = Guid.NewGuid().ToString();
            var basket = this.WhenItemAddedToBasket(userId, unicornId);
            basket.Items.Should().HaveCount(2, "because the item was added to user basket");
        }

        [Fact]
        public void AddToBasket_UserNotFound_UserAndItemAdded()
        {
            this.GivenDynamoDbRepository();
            this.GivenDynamoDBService();
            var userId = Guid.NewGuid().ToString();
            var unicornId = Guid.NewGuid().ToString();
            var basket = this.WhenItemAddedToBasket(userId, unicornId);
            this.fakeDynamoDb.Should().NotBeEmpty("because a new basket was created for the user");
            basket.Items.Should().NotBeEmpty().And.HaveCount(1, "because the item was added to user basket");
        }

        private void GivenDynamoDbRepository()
        {
            this.fakeDynamoDb = new List<Basket>();
            this.dynamoDBRepository = new Mock<IDynamoDBRepository>();

            this.dynamoDBRepository.Setup(x => x.GetDocument(It.IsAny<string>()))
                .Returns((string id) =>
                {
                    return Task.FromResult(this.fakeDynamoDb.Find(basket => basket.user_id == id));
                });

            this.dynamoDBRepository.Setup(x => x.SaveDocument(It.IsAny<Basket>()))
                .Returns((Basket basket) =>
                {
                    this.fakeDynamoDb.Add(basket);
                    return Task.FromResult(0);
                });
        }

        private string GiveUserDocumentWithItem()
        {
            var userId = Guid.NewGuid().ToString();
            this.fakeDynamoDb.Add(new Basket
            {
                user_id = userId,
                Items = new List<Item>(),
            });
            this.fakeDynamoDb.First().Items.Add(new Item
            {
                unicorn_id = Guid.NewGuid().ToString(),
                basket_id = Guid.NewGuid().ToString(),
            });

            return userId;
        }

        private void GivenDynamoDBService()
        {
            this.utility = new DynamoDBService(this.dynamoDBRepository.Object);
        }

        private List<dynamic> WhenGetBasketWithUserId(string userId)
        {
            string json = this.utility.GetBasket(userId).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<dynamic>>(json);
        }

        private Basket WhenDeleteUserBasketItem(string userId, string basketId)
        {
            string json = this.utility.DeleteBasket(userId, basketId).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Basket>(json);
        }

        private Basket WhenItemAddedToBasket(string userId, string unicornId)
        {
            string json = this.utility.AddToBasket(userId, unicornId).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Basket>(json);
        }
    }
}
