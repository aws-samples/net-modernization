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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using UnicornShopLegacy.Interfaces;
using EntityState = System.Data.Entity.EntityState;

namespace UnicornShopLegacy.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BasketController : ApiController
    {
        private IUnishopEntities unishopEntitiesContext;

        public BasketController()
        {
            this.unishopEntitiesContext = new UnishopEntities();
        }

        public BasketController(IUnishopEntities databaseContext)
        {
            this.unishopEntitiesContext = databaseContext;
        }

        // GET: api/Basket
        public IQueryable<basket> GetUnicornBaskets()
        {
            return this.unishopEntitiesContext.baskets;
        }

        // GET: api/Basket/f29b70d8-2994-4cea-861e-61903801dd98
        [ResponseType(typeof(basket))]
        public async Task<IHttpActionResult> GetUnicornBasket(Guid id)
        {
            var unicornBasket = from ub in this.unishopEntitiesContext.baskets
                                          where ub.user_id == id
                                                 select ub;

            if (unicornBasket.Count() == 0)
            {
                return this.NotFound();
            }

            return this.Ok(unicornBasket);
        }

        // PUT: api/Basket/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUnicornBasket(Guid id, basket unicornBasket)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != unicornBasket.basket_id)
            {
                return this.BadRequest();
            }

            this.unishopEntitiesContext.SetModified(unicornBasket);

            try
            {
                await this.unishopEntitiesContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UnicornBasketExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Basket
        [ResponseType(typeof(basket))]
        public async Task<IHttpActionResult> PostUnicornBasket(basket unicornBasket)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            unicornBasket.basket_id = Guid.NewGuid();
            this.unishopEntitiesContext.baskets.Add(unicornBasket);
            await this.unishopEntitiesContext.SaveChangesAsync();

            return this.CreatedAtRoute("DefaultApi", new { id = unicornBasket.basket_id }, unicornBasket);
        }

        // DELETE: api/Basket/1d6d0345-b3e5-4e0f-87a3-0a98b9a17073
        [ResponseType(typeof(basket))]
        public async Task<IHttpActionResult> DeleteUnicornBasket(Guid id)
        {
            basket unicornBasket = await this.unishopEntitiesContext.baskets.FindAsync(id);
            if (unicornBasket == null)
            {
                return this.NotFound();
            }

            this.unishopEntitiesContext.baskets.Remove(unicornBasket);
            await this.unishopEntitiesContext.SaveChangesAsync();

            return this.Ok(unicornBasket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.unishopEntitiesContext.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool UnicornBasketExists(Guid id)
        {
            return this.unishopEntitiesContext.baskets.Count(e => e.basket_id == id) > 0;
        }
    }
}