using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Interfaces;
using NerdStore.Core.Communication.Mediatr;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class ShopWindowController : Controller
    {
        private readonly IProductAppService _productAppService;
        private readonly IMediatrHandler _mediatrHandler;

        public ShopWindowController(IProductAppService productAppService,
                                    IMediatrHandler mediatrHandler)
        {
            _productAppService = productAppService;
            _mediatrHandler = mediatrHandler;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var products = await _productAppService.GetAll();
            return View(products);
        }

        [HttpGet]
        [Route("product-details/{id}")]
        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _productAppService.GetById(id);
            return View(product);
        }
    }
}