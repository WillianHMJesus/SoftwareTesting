using Api.Models;
using System;

namespace Api.Repositories
{
    public interface IProductRepository
    {
        Product GetById(Guid id);
    }
}
