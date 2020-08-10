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
    public interface IUnicornRepository
    {
        /// <summary>
        /// Fetches a list of current unicorns.
        /// </summary>
        /// <returns>List of unicorn objects. </returns>
        List<Unicorn> GetUnicorns();

        /// <summary>
        /// Fetches unicorn based on unicorn identifier.
        /// </summary>
        /// <param name="id">Unicorn Id. </param>
        /// <returns>Fetched unicorn</returns>
        Task<Unicorn> GetUnicornAsync(Guid id);

        /// <summary>
        /// Updates unicorn based on changes.
        /// </summary>
        /// <param name="unicorn">Unicorn entity to update. </param>
        /// <returns>Save status.</returns>
        Task<int> UpdateUnicornAsync(Unicorn unicorn);

        /// <summary>
        /// Creates a new unicorn entity in the context.
        /// </summary>
        /// <param name="unicorn">Unicorn entity to create. </param>
        /// <returns>Save status.</returns>
        Task<int> CreateUnicornAsync(Unicorn unicorn);

        /// <summary>
        /// Removes unicorn from context.
        /// </summary>
        /// <param name="unicorn">Unicorn entity to delete. </param>
        /// <returns>Save status.</returns>
        Task<int> DeleteUnicornAsync(Unicorn unicorn);

        /// <summary>
        /// Checks if a particular Unicorn exists.
        /// </summary>
        /// <param name="id"> Unicorn Id to search. </param>
        /// <returns></returns>
        bool UnicornExists(Guid id);
    }
}
