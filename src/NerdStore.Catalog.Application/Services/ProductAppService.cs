using AutoMapper;
using NerdStore.Catalog.Application.Dtos;
using NerdStore.Catalog.Application.Interfaces;
using NerdStore.Catalog.Domain.Interfaces;
using NerdStore.Catalog.Domain.Models;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public ProductAppService(IProductRepository productRepository,
                                 IStockService stockService,
                                 IMapper mapper)
        {
            _productRepository = productRepository;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task Add(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _productRepository.Add(product);
            await _productRepository.UnitOfWork.Commit();
        }

        public async Task Update(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _productRepository.Update(product);
            await _productRepository.UnitOfWork.Commit();
        }

        public async Task<ProductDto> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategory(int code)
        {
            var products = await _productRepository.GetByCategory(code);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await _productRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _productRepository.GetAllCategories();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<ProductDto> DebitStock(Guid id, int quantity)
        {
            if (!_stockService.DebitStock(id, quantity).Result) 
            { 
                throw new DomainException("Failed to debit stock."); 
            }

            var product = await _productRepository.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> SupplyStock(Guid id, int quantity)
        {
            if (!_stockService.SupplyStock(id, quantity).Result)
            {
                throw new DomainException("Failed to supply stock.");
            }

            var product = await _productRepository.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public void Dispose()
        {
            _productRepository.Dispose();
            _stockService.Dispose();
        }
    }
}
