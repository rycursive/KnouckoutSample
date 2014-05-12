using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KnockoutSample.DAL;
using KnockoutSample.Model;
using KnockoutSample.Service;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;

namespace KnockoutSample.Web.Controllers.api
{
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ProductsController(
            IUnitOfWorkAsync unitOfWorkAsync,
            IProductService customerService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _productService = customerService;
        }

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return _productService.ODataQueryable();
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await _productService.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            product.ObjectState = ObjectState.Modified;
            _productService.Update(product);

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.ObjectState = ObjectState.Added;
            _productService.Insert(product);
            await _unitOfWorkAsync.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await _productService.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.ObjectState = ObjectState.Deleted;
            _productService.Delete(product);
            await _unitOfWorkAsync.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWorkAsync.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _productService.Query(e => e.Id == id).Select().Any();
        }
    }
}