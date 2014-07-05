using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnockoutSample.DTO;
using KnockoutSample.Model;
using KnockoutSample.Service;
using Omu.ValueInjecter;
using Repository.Pattern.Infrastructure;

namespace KnouckoutSample.WebBridge
{
    public interface IProductWebBridge
    {
        IQueryable<ProductDTO> Queryable();
        Task<ProductDTO> FindAsync(int id);
        void Update(ProductDTO productDto);
        void Insert(ProductDTO productDto);
        void Delete(int id);
        bool ProductExists(int id);
    }
    public class ProductWebBridge : IProductWebBridge
    {
        private readonly IProductService _productService;
        public ProductWebBridge(IProductService productService)
        {
            _productService = productService;
        }
        public IQueryable<ProductDTO> Queryable()
        {
            var products = _productService.ODataQueryable().ToList();
            return products.Select(p => new ProductDTO().InjectFrom(p)).Cast<ProductDTO>().AsQueryable();
        }

        public async Task<ProductDTO> FindAsync(int id)
        {
            var product = await _productService.FindAsync(id);
            var productDto = new ProductDTO();
            productDto.InjectFrom(product);
            return productDto;
        }

        public void Update(ProductDTO productDto)
        {
            var product = new Product();
            product.InjectFrom(productDto);
            product.ObjectState = ObjectState.Modified;
            _productService.Update(product);
        }

        public void Insert(ProductDTO productDto)
        {
            var product = new Product();
            product.InjectFrom(productDto);
            product.ObjectState = ObjectState.Added;
            _productService.Insert(product);
        }

        public void Delete(int id)
        {
            _productService.Delete(id);
        }

        public bool ProductExists(int id)
        {
            return _productService.Query(e => e.Id == id).Select().Any();
        }
    }
}
