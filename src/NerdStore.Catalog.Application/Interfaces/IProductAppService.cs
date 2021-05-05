using NerdStore.Catalog.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Application.Interfaces
{
    public interface IProductAppService : IDisposable
    {
        Task Add(ProductDto productDto);
        Task Update(ProductDto productDto);

        Task<ProductDto> GetById(Guid id);
        Task<IEnumerable<ProductDto>> GetByCategory(int code);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<CategoryDto>> GetAllCategories();

        Task<ProductDto> DebitStock(Guid id, int quantity);
        Task<ProductDto> SupplyStock(Guid id, int quantity);
    }
}
