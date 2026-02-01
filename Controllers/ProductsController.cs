using Microsoft.AspNetCore.Mvc;
using Prueba_Completa.DTOs;
using Prueba_Completa.Services;

namespace Prueba_Completa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productsService;

        public ProductsController(ProductService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductoDTO>>>> GetProductos()
        {
            var response = await _productsService.GetAllProductosAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductoDTO>>> GetProducto(long id)
        {
            var response = await _productsService.GetProductoByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductoDTO>>> CreateProducto(ProductoCreateDTO dto)
        {
            var response = await _productsService.CreateProductoAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetProducto), new { id = response.Data?.ProductoId }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductoDTO>>> UpdateProducto(long id, ProductoCreateDTO dto)
        {
            var response = await _productsService.UpdateProductoAsync(id, dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
