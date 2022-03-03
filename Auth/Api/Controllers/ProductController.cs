using Core.Interfaces;
using Core.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IGenericService<Core.Models.Entities.Product, Core.Models.Dto.Product> _productService;

        public ProductController(IGenericService<Core.Models.Entities.Product, Core.Models.Dto.Product> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            return ActionResultInstance(await _productService.AddAsync(product));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            return ActionResultInstance(await _productService.UpdateAsync(product.Id, product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return ActionResultInstance(await _productService.RemoveAsync(id));
        }
    }
}