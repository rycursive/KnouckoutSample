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
using KnockoutSample.DTO;
using KnockoutSample.Service;
using KnouckoutSample.WebBridge;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;

namespace KnockoutSample.Web.Controllers.api
{
    public class ProductsController : ApiController
    {
        //private readonly IProductService _productService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IProductWebBridge _productWebBridge;

        public ProductsController(IUnitOfWorkAsync unitOfWorkAsync, IProductService customerService, IProductWebBridge productWebBridge)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            //_productService = customerService;
            _productWebBridge = productWebBridge;
        }

        // GET: api/Products
        public IQueryable<ProductDTO> GetProducts()
        {
            //return _productService.ODataQueryable();
            return _productWebBridge.Queryable();
        }

        // GET: api/Products/5
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            //Product product = await _productService.FindAsync(id);
            ProductDTO productDto = await _productWebBridge.FindAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }

            return Ok(productDto);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productDto.Id)
            {
                return BadRequest();
            }

            //product.ObjectState = ObjectState.Modified;
            //_productService.Update(product);
            _productWebBridge.Update(productDto);

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
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> PostProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //product.ObjectState = ObjectState.Added;
            //_productService.Insert(product);
            _productWebBridge.Insert(productDto);
            await _unitOfWorkAsync.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = productDto.Id }, productDto);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            //Product product = await _productService.FindAsync(id);
            ProductDTO productDto = await _productWebBridge.FindAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }

            //product.ObjectState = ObjectState.Deleted;
            //_productService.Delete(product);
            _productWebBridge.Delete(id);
            await _unitOfWorkAsync.SaveChangesAsync();

            return Ok(productDto);
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
            //return _productService.Query(e => e.Id == id).Select().Any();
            return _productWebBridge.ProductExists(id);
        }
    }
}