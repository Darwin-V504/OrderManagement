using Microsoft.AspNetCore.Mvc;
using Prueba_Completa.DTOs;
using Prueba_Completa.Services;

namespace Prueba_Completa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
      
      
            private readonly ClientService _clientService;

            public ClientsController(ClientService clientService)
            {
                _clientService = clientService;
            }

            [HttpGet]
            public async Task<ActionResult<ApiResponse<List<ClienteDTO>>>> GetClientes()
            {
                var response = await _clientService.GetAllClientesAsync();
                return Ok(response);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ApiResponse<ClienteDTO>>> GetCliente(long id)
            {
                var response = await _clientService.GetClienteByIdAsync(id);

                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            }

            [HttpPost]
            public async Task<ActionResult<ApiResponse<ClienteDTO>>> CreateCliente(ClienteCreateDTO dto)
            {
                var response = await _clientService.CreateClienteAsync(dto);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return CreatedAtAction(nameof(GetCliente), new { id = response.Data?.ClienteId }, response);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult<ApiResponse<ClienteDTO>>> UpdateCliente(long id, ClienteUpdateDTO dto)
            {
                var response = await _clientService.UpdateClienteAsync(id, dto);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
    }
}

