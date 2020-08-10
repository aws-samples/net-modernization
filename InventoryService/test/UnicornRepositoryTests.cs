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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Data;
using InventoryService.Interface;
using InventoryService.Models;
using InventoryService.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InventoryService.Tests
{
    [TestClass]
    public class UnicornRepositoryTests
    {
        private IUnicornRepository unicornRepository;
        private List<Unicorn> fakeUnicorns;
        private Mock<UnicornShop> fakeContext;
        private Mock<DbSet<Unicorn>> fakeDbSet;

        [TestMethod]
        public void GetUnicorns_WhenCalled_VerifyContextUnicorns()
        {
            this.GivenUnicornDb();
            this.GivenUnicornShopContext();
            this.GivenRepository();
            var unicorns = this.unicornRepository.GetUnicorns();
            this.fakeContext.Verify(m => m.Unicorns, Times.Once);
        }

        [TestMethod]
        public void GetUnicornAsync_UnicornId_VerifyContextFindAsync()
        {
            this.GivenUnicornDb();
            this.GivenUnicornShopContext();
            this.GivenRepository();
            var unicornId = this.fakeUnicorns.First().unicorn_id;
            var unicorn = this.unicornRepository.GetUnicornAsync(unicornId).Result;
            this.fakeContext.Verify(m => m.Unicorns.FindAsync(unicornId), Times.Once);
        }

        [TestMethod]
        public void UpdateUnicornAsync_VerifyContext_SetModifiedSaveChanges()
        {
            this.GivenUnicornDb();
            this.GivenUnicornShopContext();
            this.GivenRepository();

            var unicorn = new Unicorn();
            var status = this.unicornRepository.UpdateUnicornAsync(unicorn).Result;
            this.fakeContext.Verify(m => m.SetModified(unicorn), Times.Once);
            this.fakeContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public void CreateUnicornAsync_VerifyContext_AddSaveChanges()
        {
            this.GivenUnicornDb();
            this.GivenUnicornShopContext();
            this.GivenRepository();

            var unicorn = new Unicorn();
            var status = this.unicornRepository.CreateUnicornAsync(unicorn).Result;
            this.fakeContext.Verify(m => m.Unicorns.Add(unicorn), Times.Once);
            this.fakeContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public void DeleteUnicornAsync_VerifyContext_RemoveSaveChanges()
        {
            this.GivenUnicornDb();
            this.GivenUnicornShopContext();
            this.GivenRepository();

            var unicorn = new Unicorn();
            var status = this.unicornRepository.DeleteUnicornAsync(unicorn).Result;
            this.fakeContext.Verify(m => m.Unicorns.Remove(unicorn), Times.Once);
            this.fakeContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        private void GivenUnicornDb()
        {
            this.fakeUnicorns = new List<Unicorn>
            {
                new Unicorn { unicorn_id = Guid.NewGuid() },
                new Unicorn { unicorn_id = Guid.NewGuid() },
                new Unicorn { unicorn_id = Guid.NewGuid() },
            };

            var queryableUnicorns = this.fakeUnicorns.AsQueryable();

            this.fakeDbSet = new Mock<DbSet<Unicorn>>();

            this.fakeDbSet.As<IQueryable<Unicorn>>().Setup(m => m.Provider).Returns(queryableUnicorns.Provider);
            this.fakeDbSet.As<IQueryable<Unicorn>>().Setup(m => m.Expression).Returns(queryableUnicorns.Expression);
            this.fakeDbSet.As<IQueryable<Unicorn>>().Setup(m => m.ElementType).Returns(queryableUnicorns.ElementType);
            this.fakeDbSet.As<IQueryable<Unicorn>>().Setup(m => m.GetEnumerator()).Returns(queryableUnicorns.GetEnumerator());
        }

        private void GivenUnicornShopContext()
        {
            this.fakeContext = new Mock<UnicornShop>();
            this.fakeContext.Setup(x => x.Unicorns).Returns(this.fakeDbSet.Object);
        }

        private void GivenRepository()
        {
            this.unicornRepository = new UnicornRepository(this.fakeContext.Object);
        }
    }
}
