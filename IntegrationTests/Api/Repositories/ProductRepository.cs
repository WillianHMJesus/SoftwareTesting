using Api.Models;
using System;
using System.Linq;

namespace Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Product GetById(Guid id)
        {
            return Product.Products.FirstOrDefault(x => x.Id == id);
        }
    }
}
