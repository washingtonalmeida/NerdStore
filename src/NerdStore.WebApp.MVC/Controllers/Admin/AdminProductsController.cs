using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Dtos;
using NerdStore.Catalog.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers.Admin
{
    public class AdminProductsController : Controller
    {
        private readonly IProductAppService _productAppService;

        public AdminProductsController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        [Route("admin-products")]
        public async Task<IActionResult> Index()
        {
            var products = await _productAppService.GetAll();
            return View(products);
        }

        [HttpGet]
        [Route("add-product")]
        public async Task<IActionResult> AddProduct()
        {
            var product = await FillCategories(new ProductDto());
            return View(product);
        }

        [HttpPost]
        [Route("add-product")]
        public async Task<IActionResult> AddProduct(ProductDto product)
        {
            if (!ModelState.IsValid)
                return View(await FillCategories(product));

            await _productAppService.Add(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("update-product")]
        public async Task<IActionResult> UpdateProduct(Guid id)
        {
            var product = await FillCategories(await _productAppService.GetById(id));
            return View(product);
        }

        [HttpPost]
        [Route("update-product")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductDto product)
        {
            product.StockQuantity = _productAppService.GetById(id).Result.StockQuantity;

            ModelState.Remove("StockQuantity");
            if (!ModelState.IsValid)
                return View(await FillCategories(product));

            await _productAppService.Update(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("update-product-stock")]
        public async Task<IActionResult> UpdateProductStock(Guid id)
        {
            var product = await _productAppService.GetById(id);
            return View("Stock", product);
        }

        [HttpPost]
        [Route("update-product-stock")]
        public async Task<IActionResult> UpdateProductStock(Guid id, int quantity)
        {
            if (quantity > 0) {
                await _productAppService.SupplyStock(id, quantity);
            }
            else
            {
                await _productAppService.DebitStock(id, quantity);
            }

            var products = await _productAppService.GetAll();

            return View("Index", products);
        }

        private async Task<ProductDto> FillCategories(ProductDto product)
        {
            product.Categories = await _productAppService.GetAllCategories();
            return product;
        }
    }
}
