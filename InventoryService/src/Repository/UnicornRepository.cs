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
using InventoryService.Data;
using InventoryService.Interface;
using InventoryService.Models;

namespace InventoryService.Repository
{
    public class UnicornRepository : IUnicornRepository
    {
        private readonly UnicornShop unicornShopDbContext;

        public UnicornRepository(UnicornShop context)
        {
            this.unicornShopDbContext = context;
        }

        /// <inheritdoc/>
        public async Task<int> CreateUnicornAsync(Unicorn unicorn)
        {
            this.unicornShopDbContext.Unicorns.Add(unicorn);
            return await this.unicornShopDbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteUnicornAsync(Unicorn unicorn)
        {
            this.unicornShopDbContext.Unicorns.Remove(unicorn);
            return await this.unicornShopDbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Unicorn> GetUnicornAsync(Guid id)
        {
            return await this.unicornShopDbContext.Unicorns.FindAsync(id);
        }

        /// <inheritdoc/>
        public List<Unicorn> GetUnicorns()
        {
            return this.unicornShopDbContext.Unicorns.ToList();
        }

        /// <inheritdoc/>
        public bool UnicornExists(Guid id)
        {
            return this.unicornShopDbContext.Unicorns.Any(e => e.unicorn_id == id);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateUnicornAsync(Unicorn unicorn)
        {
            this.unicornShopDbContext.SetModified(unicorn);
            return await this.unicornShopDbContext.SaveChangesAsync();
        }
    }
}
