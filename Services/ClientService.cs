using Microsoft.EntityFrameworkCore;
using Prueba_Completa.Data;
using Prueba_Completa.DTOs;
using Prueba_Completa.Models;

namespace Prueba_Completa.Services
{
    public class ClientService
    {
        private readonly ApplicationDBContext _context;

        public ClientService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ClienteDTO>>> GetAllClientesAsync()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Select(c => new ClienteDTO
                    {
                        ClienteId = c.ClienteId,
                        Nombre = c.Nombre,
                       Identidad = c.Identidad
                    })
                    .ToListAsync();

                return new ApiResponse<List<ClienteDTO>>
                {
                    Success = true,
                    Message = "",
                    Errors = new List<string>(),
                    Data = clientes
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ClienteDTO>>
                {
                    Success = false,
                    Message = "Error al obtener clientes",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ClienteDTO>> GetClienteByIdAsync(long id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return new ApiResponse<ClienteDTO>
                    {
                        Success = false,
                        Message = "Cliente no encontrado",
                        Errors = new List<string> { "No existe un cliente con el ID especificado" },
                        Data = null
                    };
                }

                var clienteDTO = new ClienteDTO
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Identidad = cliente.Identidad
                };

                return new ApiResponse<ClienteDTO>
                {
                    Success = true,
                    Message = "",
                    Errors = new List<string>(),
                    Data = clienteDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClienteDTO>
                {
                    Success = false,
                    Message = "Error al obtener cliente",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ClienteDTO>> CreateClienteAsync(ClienteCreateDTO dto)
        {
            try
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    errors.Add("Nombre es requerido");
                else if (dto.Nombre.Length < 3 || dto.Nombre.Length > 100)
                    errors.Add("Nombre debe tener entre 3 y 100 caracteres");

                if (string.IsNullOrWhiteSpace(dto.Identidad))
                    errors.Add("Identidad es requerida");
                else if (dto.Identidad.Length < 5 || dto.Identidad.Length > 50)
                    errors.Add("Identidad debe tener un formato válido");

                var existeIdentidad = await _context.Clientes
                    .AnyAsync(c => c.Identidad == dto.Identidad);

                if (existeIdentidad)
                    errors.Add($"Ya existe un cliente con la identidad {dto.Identidad}");

                if (errors.Count > 0)
                {
                    return new ApiResponse<ClienteDTO>
                    {
                        Success = false,
                        Message = "Error al crear el cliente",
                        Errors = errors,
                        Data = null
                    };
                }

                var cliente = new Cliente
                {
                    Nombre = dto.Nombre,
                    Identidad = dto.Identidad,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                var clienteDTO = new ClienteDTO
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Identidad = cliente.Identidad
                };

                return new ApiResponse<ClienteDTO>
                {
                    Success = true,
                    Message = "Cliente creado exitosamente",
                    Errors = new List<string>(),
                    Data = clienteDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClienteDTO>
                {
                    Success = false,
                    Message = "Error al crear el cliente",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ClienteDTO>> UpdateClienteAsync(long id, ClienteUpdateDTO dto)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return new ApiResponse<ClienteDTO>
                    {
                        Success = false,
                        Message = "Cliente no encontrado",
                        Errors = new List<string> { "No existe un cliente con el ID especificado" },
                        Data = null
                    };
                }

                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    errors.Add("Nombre es requerido");
                else if (dto.Nombre.Length < 3 || dto.Nombre.Length > 100)
                    errors.Add("Nombre debe tener entre 3 y 100 caracteres");

                if (string.IsNullOrWhiteSpace(dto.Identidad))
                    errors.Add("Identidad es requerida");
                else if (dto.Identidad.Length < 5 || dto.Identidad.Length > 50)
                    errors.Add("Identidad debe tener un formato válido");

                var existeIdentidad = await _context.Clientes
                    .AnyAsync(c => c.Identidad == dto.Identidad && c.ClienteId != id);

                if (existeIdentidad)
                    errors.Add($"Ya existe otro cliente con la identidad {dto.Identidad}");

                if (errors.Count > 0)
                {
                    return new ApiResponse<ClienteDTO>
                    {
                        Success = false,
                        Message = "Error al actualizar el cliente",
                        Errors = errors,
                        Data = null
                    };
                }

                cliente.Nombre = dto.Nombre;
                cliente.Identidad = dto.Identidad;

                await _context.SaveChangesAsync();

                var clienteDTO = new ClienteDTO
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Identidad = cliente.Identidad
                };

                return new ApiResponse<ClienteDTO>
                {
                    Success = true,
                    Message = "Cliente actualizado exitosamente",
                    Errors = new List<string>(),
                    Data = clienteDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClienteDTO>
                {
                    Success = false,
                    Message = "Error al actualizar el cliente",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }
    }
}

