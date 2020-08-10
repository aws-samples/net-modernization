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
using InventoryService.Models;

namespace InventoryService.Interface
{
    public interface IUnicornService : IUnicornRepository
    {
        /// <summary>
        /// Fetches the unicorn to update and updates the entity.
        /// </summary>
        /// <param name="id">Unicorn Id. </param>
        /// <param name="unicorn">Unicorn entity to update. </param>
        /// <returns>Save status. </returns>
        Task<int?> FindUnicornUpdateAsync(Guid id, Unicorn unicorn);

        /// <summary>
        /// Fetches the unicorn to delete and updates the context.
        /// </summary>
        /// <param name="id">Unicorn Id.</param>
        /// <returns>Save status. </returns>
        #nullable enable
        Task<Unicorn?> FindUnicornDeleteAsync(Guid id);
        #nullable disable
        /// <summary>
        /// Creates new item in database according to user upload if its content is appropriate.
        /// </summary>
        /// <param name="upload">Uploaded unicorn obj and image as byte[]</param>
        /// <returns>Save status. </returns>
        Task<int?> CreateItemAsync(NewUploadedItem upload);
    }
}
