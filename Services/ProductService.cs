using Microsoft.EntityFrameworkCore;
using Prueba_Completa.Data;
using Prueba_Completa.Models;
using Prueba_Completa.DTOs;

namespace Prueba_Completa.Services
{
    public class ProductService
    {
        private readonly ApplicationDBContext _context;

        public ProductService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ProductoDTO>>> GetAllProductosAsync()
        {
            try
            {
                var productos = await _context.Productos
                    .Select(p => new ProductoDTO
                    {
                        ProductoId = p.ProductoId,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        Existencia = p.Existencia
                    })
                    .ToListAsync();

                return new ApiResponse<List<ProductoDTO>>
                {
                    Success = true,
                    Message = "",
                    Errors = new List<string>(),
                    Data = productos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ProductoDTO>>
                {
                    Success = false,
                    Message = "Error al obtener productos",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ProductoDTO>> GetProductoByIdAsync(long id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto == null)
                {
                    return new ApiResponse<ProductoDTO>
                    {
                        Success = false,
                        Message = "Producto no encontrado",
                        Errors = new List<string> { "No existe un producto con el ID especificado" },
                        Data = null
                    };
                }

                var productoDTO = new ProductoDTO
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Existencia = producto.Existencia
                };

                return new ApiResponse<ProductoDTO>
                {
                    Success = true,
                    Message = "",
                    Errors = new List<string>(),
                    Data = productoDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductoDTO>
                {
                    Success = false,
                    Message = "Error al obtener producto",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ProductoDTO>> CreateProductoAsync(ProductoCreateDTO dto)
        {
            try
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    errors.Add("Nombre es requerido");
                else if (dto.Nombre.Length < 3 || dto.Nombre.Length > 100)
                    errors.Add("Nombre debe tener entre 3 y 100 caracteres");

                if (dto.Precio <= 0)
                    errors.Add("Precio debe ser mayor a 0");

                if (dto.Existencia < 0)
                    errors.Add("Existencia debe ser mayor o igual a 0");

                if (errors.Count > 0)
                {
                    return new ApiResponse<ProductoDTO>
                    {
                        Success = false,
                        Message = "Error al crear el producto",
                        Errors = errors,
                        Data = null
                    };
                }

                var producto = new Producto
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    Existencia = dto.Existencia,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                var productoDTO = new ProductoDTO
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Existencia = producto.Existencia
                };

                return new ApiResponse<ProductoDTO>
                {
                    Success = true,
                    Message = "Producto creado exitosamente",
                    Errors = new List<string>(),
                    Data = productoDTO
                };
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " | Inner: " + ex.InnerException.Message;

                return new ApiResponse<ProductoDTO>
                {
                    Success = false,
                    Message = "Error al crear el producto",
                    Errors = new List<string> { errorMessage },
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ProductoDTO>> UpdateProductoAsync(long id, ProductoCreateDTO dto)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto == null)
                {
                    return new ApiResponse<ProductoDTO>
                    {
                        Success = false,
                        Message = "Producto no encontrado",
                        Errors = new List<string> { "No existe un producto con el ID especificado" },
                        Data = null
                    };
                }

                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    errors.Add("Nombre es requerido");
                else if (dto.Nombre.Length < 3 || dto.Nombre.Length > 100)
                    errors.Add("Nombre debe tener entre 3 y 100 caracteres");

                if (dto.Precio <= 0)
                    errors.Add("Precio debe ser mayor a 0");

                if (dto.Existencia < 0)
                    errors.Add("Existencia debe ser mayor o igual a 0");

                if (errors.Count > 0)
                {
                    return new ApiResponse<ProductoDTO>
                    {
                        Success = false,
                        Message = "Error al actualizar el producto",
                        Errors = errors,
                        Data = null
                    };
                }

                producto.Nombre = dto.Nombre;
                producto.Descripcion = dto.Descripcion;
                producto.Precio = dto.Precio;
                producto.Existencia = dto.Existencia;

                await _context.SaveChangesAsync();

                var productoDTO = new ProductoDTO
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Existencia = producto.Existencia
                };

                return new ApiResponse<ProductoDTO>
                {
                    Success = true,
                    Message = "Producto actualizado exitosamente",
                    Errors = new List<string>(),
                    Data = productoDTO
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductoDTO>
                {
                    Success = false,
                    Message = "Error al actualizar el producto",
                    Errors = new List<string> { ex.Message },
                    Data = null
                };
            }
        }
    }
}