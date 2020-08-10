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
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InventoryService.Data;
using InventoryService.Interface;
using InventoryService.Models;
using InventoryService.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InventoryService.Tests
{
    [TestClass]
    public class UnicornServiceTests
    {
        private Mock<IUnicornRepository> fakeUnicornRepository;
        private Mock<IS3UnicornRepository> fakeS3UnicornRepository;
        private Mock<UnicornShop> fakeContext;
        private IUnicornService unicornService;
        private Guid unicornId;

        [TestMethod]
        public void FindUnicornUpdateAsync_UnicornFound_ReturnStatus()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var status = this.unicornService.FindUnicornUpdateAsync(this.unicornId, new Unicorn()).Result;
            status.Value.Should().NotBe(null, "Because the unicorn Id exists");
        }

        [TestMethod]
        public void FindUnicornUpdateAsync_UnicorNotFound_ReturnStatus()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var status = this.unicornService.FindUnicornUpdateAsync(Guid.NewGuid(), new Unicorn()).Result;
            status.Should().BeNull(null, "Because the unicorn Id does not exist");
        }

        [TestMethod]
        public void CreateItemAsync_S3Upload_OK()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var unicorn = new Unicorn();
            var status = this.unicornService.CreateItemAsync(new NewUploadedItem { Unicorn = unicorn });
            status.Result.Should().Be(1, "because the unicorn image was uploaded to S3");
        }

        [TestMethod]
        public void CreateItemAsync_S3Upload_NotOK()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var unicorn = new Unicorn { image = "uploadFail" };
            var status = this.unicornService.CreateItemAsync(new NewUploadedItem { Unicorn = unicorn });
            status.Result.Should().Be(null, "because the unicorn image was not uploaded to S3");
        }

        [TestMethod]
        public void FindUnicornDeleteAsync_UnicornFound_ReturnStatus()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var unicorn = this.unicornService.FindUnicornDeleteAsync(this.unicornId).Result;
            unicorn.Should().NotBe(null, "Because the unicorn Id exists")
                .And.BeOfType<Unicorn>();
        }

        [TestMethod]
        public void FindUnicornDeleteAsync_UnicornNotFound_ReturnStatus()
        {
            this.GivenS3UnicornRepository();
            this.GivenUnicornRepository();
            this.GivenUnicornService();
            var unicorn = this.unicornService.FindUnicornDeleteAsync(Guid.NewGuid()).Result;
            unicorn.Should().BeNull(null, "Because the unicorn Id does not exists");
        }

        private void GivenS3UnicornRepository()
        {
            this.fakeS3UnicornRepository = new Mock<IS3UnicornRepository>();
            this.fakeS3UnicornRepository.Setup(m => m.UploadImageToS3(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Task.FromResult(201));
            this.fakeS3UnicornRepository.Setup(m => m.UploadImageToS3(It.IsAny<byte[]>(), It.Is<string>(name => name == "uploadFail"))).Returns(Task.FromResult(500));
        }

        private void GivenUnicornRepository()
        {
            this.unicornId = Guid.NewGuid();
            this.fakeUnicornRepository = new Mock<IUnicornRepository>();
            this.fakeUnicornRepository.Setup(m => m.GetUnicornAsync(It.IsAny<Guid>())).Returns(Task.FromResult((Unicorn)null));
            this.fakeUnicornRepository.Setup(m => m.GetUnicornAsync(It.Is<Guid>(unicornId => unicornId == this.unicornId)))
                .Returns(Task.FromResult(new Unicorn() { unicorn_id = this.unicornId }));
            this.fakeUnicornRepository.Setup(m => m.UnicornExists(It.IsAny<Guid>())).Returns(false);
            this.fakeUnicornRepository.Setup(m => m.UnicornExists(It.Is<Guid>(unicornId => unicornId == this.unicornId)))
                .Returns(true);
            this.fakeUnicornRepository.Setup(m => m.DeleteUnicornAsync(It.IsAny<Unicorn>())).Returns(Task.FromResult(1));
            this.fakeUnicornRepository.Setup(m => m.UpdateUnicornAsync(It.IsAny<Unicorn>())).Returns(Task.FromResult(1));
            this.fakeUnicornRepository.Setup(m => m.CreateUnicornAsync(It.IsAny<Unicorn>())).Returns(Task.FromResult(1));
        }

        private void GivenUnicornService()
        {
            this.fakeContext = new Mock<UnicornShop>();
            this.unicornService = new UnicornService(this.fakeContext.Object, this.fakeUnicornRepository.Object, this.fakeS3UnicornRepository.Object);
        }
    }
}
