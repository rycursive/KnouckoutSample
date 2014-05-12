using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnockoutSample.Model;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace KnockoutSample.Service
{
    public interface IProductService : IService<Product>
    {
    }

    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IRepositoryAsync<Product> repository)
            : base(repository)
        {
        }
    }
}
