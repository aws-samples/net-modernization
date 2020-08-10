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
using InventoryService.Data;
using InventoryService.Interface;
using InventoryService.Models;
using InventoryService.Repository;

namespace InventoryService.Service
{
    public class UnicornService : UnicornRepository, IUnicornService
    {
        private readonly IUnicornRepository unicornRepository;
        private readonly IS3UnicornRepository s3UnicornRepository;

        public UnicornService(UnicornShop context, IUnicornRepository unicornRepository, IS3UnicornRepository s3UnicornRepository)
            : base(context)
        {
            this.unicornRepository = unicornRepository;
            this.s3UnicornRepository = s3UnicornRepository;
        }

        /// <inheritdoc/>
        public async Task<int?> FindUnicornUpdateAsync(Guid id, Unicorn unicorn)
        {
            if (!this.unicornRepository.UnicornExists(id))
            {
                return null;
            }

            return await this.unicornRepository.UpdateUnicornAsync(unicorn);
        }

        /// <inheritdoc/>
        public async Task<int?> CreateItemAsync(NewUploadedItem upload)
        {
            var s3UploadResponse = await this.s3UnicornRepository.UploadImageToS3(upload.Image, upload.Unicorn.image);
            if (s3UploadResponse != 200 & s3UploadResponse != 201)
            {
                return null;
            }

            return await this.unicornRepository.CreateUnicornAsync(upload.Unicorn);
        }

        /// <inheritdoc/>
        #nullable enable
        public async Task<Unicorn?> FindUnicornDeleteAsync(Guid id)
        {
            var unicorn = await this.unicornRepository.GetUnicornAsync(id);

            if (unicorn == null)
            {
                return null;
            }

            await this.unicornRepository.DeleteUnicornAsync(unicorn);

            return unicorn;
        }
        #nullable disable

    }
}