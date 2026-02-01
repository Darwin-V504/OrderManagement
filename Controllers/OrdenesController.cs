using Microsoft.AspNetCore.Mvc;
using Prueba_Completa.DTOs;
using Prueba_Completa.Services;

namespace Prueba_Completa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenService _ordenService;

        public OrdenesController(OrdenService ordenService)
        {
            _ordenService = ordenService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrdenDTO>>> CreateOrden(OrdenCreateDTO dto)
        {
            var response = await _ordenService.CreateOrdenAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Created("", response);
        }
    }
}
