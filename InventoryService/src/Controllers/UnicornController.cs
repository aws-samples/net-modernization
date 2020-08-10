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
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnicornController : ControllerBase
    {
        private readonly IUnicornService unicornService;
        private readonly IRekognitionService rekognitionService;

        public UnicornController(IUnicornService unicornService, IRekognitionService rekognitionService)
        {
            this.unicornService = unicornService;
            this.rekognitionService = rekognitionService;
        }

        /// <summary>
        /// GET: api/Unicorn
        /// </summary>
        /// <returns> List of Unicorn entities. </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unicorn>>> GetUnicorns()
        {
            return this.unicornService.GetUnicorns();
        }

        /// <summary>
        /// GET: api/Unicorn/1d6d0345-b3e5-4e0f-87a3-0a98b9a17222
        /// </summary>
        /// <param name="id"> Unicorn Id to fetch a unicorn. </param>
        /// <returns> Unicorn entity. </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Unicorn>> GetUnicorn(Guid id)
        {
            var unicorn = await this.unicornService.GetUnicornAsync(id);

            if (unicorn == null)
            {
                return this.NotFound();
            }

            return unicorn;
        }

        /// <summary>
        /// PUT: api/Unicorn/1d6d0345-b3e5-4e0f-87a3-0a98b9a17222
        /// </summary>
        /// <param name="id"> Unicorn Id to update. </param>
        /// <param name="unicorn"> Modified unicorn entity. </param>
        /// <returns> No content with status code 204. </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnicorn(Guid id, Unicorn unicorn)
        {
            if (id != unicorn.unicorn_id)
            {
                return this.BadRequest();
            }

            var status = await this.unicornService.FindUnicornUpdateAsync(id, unicorn);

            if (status == null)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// POST: api/Unicorn
        /// </summary>
        /// <param name="unicorn"> New unicorn entity. </param>
        /// <returns> Reroute to new unicorn. </returns>
        [HttpPost]
        public async Task<ActionResult<Unicorn>> CreateUnicorn(NewUploadedItem upload)
        {
            // Uncomment the code below for the AI Content Moderation extra credit lab
            /*
            var rekognitionResponse = await this.rekognitionService.GetContentModerationLabels(upload.Image);
            if (rekognitionResponse != null)
            {
                return this.BadRequest(rekognitionResponse);
            }
            */

            var createItemResponse = await this.unicornService.CreateItemAsync(upload);
            if (createItemResponse == null)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction("GetUnicorn", new { id = upload.Unicorn.unicorn_id }, upload.Unicorn);
        }

        /// <summary>
        /// DELETE: api/Unicorn/1d6d0345-b3e5-4e0f-87a3-0a98b9a17222
        /// </summary>
        /// <param name="id"> Unicorn Id to delete. </param>
        /// <returns> Deleted unicorn entity. </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unicorn>> DeleteUnicorn(Guid id)
        {
            var unicorn = await this.unicornService.FindUnicornDeleteAsync(id);

            if (unicorn == null)
            {
                return this.NotFound();
            }

            return unicorn;
        }
    }
}
