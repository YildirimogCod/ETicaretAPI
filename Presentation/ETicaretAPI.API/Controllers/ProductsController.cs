using System.Net;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParam;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IValidator<CreateProduct> _validator;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository,IValidator<CreateProduct> validator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _validator = (IValidator<CreateProduct>)validator;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetlAll(false).Count();
            var products = _productReadRepository.GetlAll(false).Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Price,
                    x.Stock,
                    x.CreatedDate,
                    x.UpdatedDate,

            }
            ).Skip(pagination.Page * pagination.Size).Take(pagination.Size).ToList();
            return Ok(new
            {
                totalCount,
                products

            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id,false);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateProduct product)
        {
            if (product == null)
            {
                return BadRequest("Geçersiz ürün verisi.");
            }

            var validationResult = await _validator.ValidateAsync(product);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);  // Eğer model geçersizse, hata mesajlarını döndürür
            }

            await _productWriteRepository.AddAsync(new()
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)(HttpStatusCode.Created));
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            _productWriteRepository.Update(product);
            await _productWriteRepository.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            _productWriteRepository.Remove(product);
            await _productWriteRepository.SaveAsync();
            return NoContent();
        }

    }
}
