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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Models
{
    public class Unicorn
    {
        /// <summary>
        /// Unique identifier for each unicorn.
        /// </summary>
        [Key]
        public System.Guid unicorn_id { get; set; }

        /// <summary>
        /// Name of the unicorn inventory item.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Description of the unicorn inventory item.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Cost of the unicorn inventory item.
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// Unicorn inventory item image name.
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// Unicorn item creation date and time
        /// </summary>
        public System.DateTime? date_create { get; set; }
    }
}
