using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Add(Product product);
        void Update(Product product);
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategory(int code);
        Task<IEnumerable<Product>> GetAll();

        void Add(Category category);
        void Update(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
    }
}
