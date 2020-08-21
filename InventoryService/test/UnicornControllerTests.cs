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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InventoryService.Controllers;
using InventoryService.Data;
using InventoryService.Interface;
using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InventoryService.Tests
{
    [TestClass]
    public class UnicornControllerTests
    {
        private UnicornController unicornController;
        private Mock<IUnicornService> fakeUnicornService;
        private Mock<IRekognitionService> fakeRekognitionService;
        private Mock<ISNSService> fakeSNSService;
        private byte[] fakeImageBytes1;
        private byte[] fakeImageBytes2;

        [TestMethod]
        public void GetUnicorns_WhenCalled_ListUnicorns()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorns = this.unicornController.GetUnicorns();
            unicorns.Value.Should().BeOfType<List<Unicorn>>("because are fetching unicorns.");
        }

        [TestMethod]
        public void GetUnicorn_UnicornId_ReturnUnicorn()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = this.unicornController.GetUnicorn(Guid.NewGuid()).Result;
            unicorn.Value.Should().BeOfType<Unicorn>("we are getting a unicorn object");
        }

        [TestMethod]
        public void UpdateUnicorn_SaveSucceed_ReturnNoContent()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = new Unicorn { unicorn_id = Guid.NewGuid() };
            var result = this.unicornController.UpdateUnicorn(unicorn.unicorn_id, unicorn).Result;
            result.Should().BeOfType<NoContentResult>("because the update was successfull.");
            result.As<NoContentResult>().StatusCode.Should().Be(204);
        }

        [TestMethod]
        public void UpdateUnicorn_DifferentUnicornId_ReturnBadRequest()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = new Unicorn { unicorn_id = Guid.NewGuid() };
            var result = this.unicornController.UpdateUnicorn(Guid.NewGuid(), unicorn).Result;
            result.Should().BeOfType<BadRequestResult>("because the unicorn Id did not match.");
            result.As<BadRequestResult>().StatusCode.Should().Be(400);
        }

        [TestMethod]
        [Ignore]
        public void CreateUnicorn_WhenCalled_CreatedActionGetUnicorn()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = new Unicorn { unicorn_id = Guid.NewGuid() };
            var uploadItem = new NewUploadedItem { Unicorn = unicorn, Image = this.fakeImageBytes1 };
            var result = this.unicornController.CreateUnicorn(uploadItem).Result.Result as CreatedAtActionResult;
            result.ActionName.Should().Be("GetUnicorn");
            result.StatusCode.Should().Be(201, "because a new inventory item was created");
        }

        /* This test will currently fail as the Rekognition logic is currently commented out and not implemented. The test will
         * continue to fail until the code indicated in the extra credit AI Content Moderdation Lab is uncommented. */
        [TestMethod]
        [Ignore]
        public void CreateUnicorn_WhenCalled_ReturnBadRequest()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = new Unicorn { unicorn_id = Guid.NewGuid() };
            var uploadItem = new NewUploadedItem { Unicorn = unicorn, Image = this.fakeImageBytes2 };
            var result = this.unicornController.CreateUnicorn(uploadItem).Result.Result;
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void DeleteUnicorn_UnicornFound_DeleteUnicornFromList()
        {
            this.GivenUnicornService();
            this.GivenRekognitionService();
            this.GivenSNSService();
            this.GivenUnicornController();
            var unicorn = this.unicornController.DeleteUnicorn(Guid.NewGuid()).Result;
            unicorn.Value.Should().NotBeNull("because we have the Unicorn in Db")
                .And.BeOfType<Unicorn>("because it is a Unicorn object.");
        }

        private void GivenUnicornService()
        {
            this.fakeUnicornService = new Mock<IUnicornService>();
            this.fakeUnicornService.Setup(m => m.GetUnicorns()).Returns(new List<Unicorn>{ new Unicorn() });
            this.fakeUnicornService.Setup(m => m.GetUnicornAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Unicorn()));
            this.fakeUnicornService.Setup(m => m.CreateItemAsync(It.IsAny<NewUploadedItem>())).Returns(Task.FromResult((int?)1));
            this.fakeUnicornService.Setup(m => m.FindUnicornUpdateAsync(It.IsAny<Guid>(), It.IsAny<Unicorn>())).Returns(Task.FromResult((int?)1));
            this.fakeUnicornService.Setup(m => m.FindUnicornDeleteAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Unicorn()));
        }

        private void GivenRekognitionService()
        {
            this.fakeImageBytes1 = new byte[] { 0x01, 0x01, 0x01, 0x01 };
            this.fakeImageBytes2 = new byte[] { 0xFF, 0xFF, 0XFF, 0XFF };
            this.fakeRekognitionService = new Mock<IRekognitionService>();
            this.fakeRekognitionService.Setup(m => m.GetContentModerationLabels(It.Is<byte[]>(uploadImage => uploadImage == this.fakeImageBytes1))).Returns((string)null);
	        this.fakeRekognitionService.Setup(m => m.GetContentModerationLabels(It.Is<byte[]>(uploadImage => uploadImage == this.fakeImageBytes2))).Returns(string.Empty);
        }

        private void GivenSNSService()
        {
            this.fakeSNSService = new Mock<ISNSService>();
            this.fakeSNSService.Setup(m => m.PublishMessageToSNSAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
        }

        private void GivenUnicornController()
        {
            this.unicornController = new UnicornController(this.fakeUnicornService.Object, this.fakeRekognitionService.Object, this.fakeSNSService.Object);
        }
    }
}
