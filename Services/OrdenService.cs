using Microsoft.EntityFrameworkCore;
using Prueba_Completa.Data;
using Prueba_Completa.Models;
using Prueba_Completa.DTOs;

namespace Prueba_Completa.Services
{
    public class OrdenService
    {
        private readonly ApplicationDBContext _context;

        public OrdenService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<OrdenDTO>> CreateOrdenAsync(OrdenCreateDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var errors = new List<string>();

                //  Validar cliente
                var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
                if (cliente == null)
                {
                    errors.Add("El cliente especificado no existe");
                }

                //  Inicializar orden
                var orden = new Orden
                {
                    ClienteId = dto.ClienteId,
                    Impuesto = 0,
                    Subtotal = 0,
                    Total = 0,
                    FechaCreacion = DateTime.UtcNow
                };

                // Solo agregar si cliente existe
                if (cliente != null)
                {
                    _context.Ordenes.Add(orden);
                    await _context.SaveChangesAsync();
                }

                decimal impuestoTotal = 0;
                decimal subtotalTotal = 0;
                decimal totalTotal = 0;
                var detalles = new List<DetalleOrden>();
                var detallesDTO = new List<DetalleOrdenDTO>();

                //  Procesar cada detalle
                foreach (var detalleDto in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalleDto.ProductoId);
                    if (producto == null)
                    {
                        errors.Add($"Producto con ID {detalleDto.ProductoId} no encontrado");
                        continue;
                    }

                    // Validar existencia
                    if (producto.Existencia < detalleDto.Cantidad)
                    {
                        errors.Add($"El producto '{producto.Nombre}' no tiene suficientes existencias. Disponible: {producto.Existencia}, Solicitado: {detalleDto.Cantidad}");
                        continue;
                    }

                    // Calcular valores
                    decimal subtotal = detalleDto.Cantidad * producto.Precio;
                    decimal impuesto = subtotal * 0.15m;
                    decimal total = subtotal + impuesto;

                    // Crear detalle
                    var detalle = new DetalleOrden
                    {
                        OrdenId = orden.OrdenId,
                        ProductoId = producto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        Subtotal = subtotal,
                        Impuesto = impuesto,
                        Total = total
                    };

                    detalles.Add(detalle);

                    // Actualizar existencia
                    producto.Existencia -= detalleDto.Cantidad;

                    // Acumular totales
                    impuestoTotal += impuesto;
                    subtotalTotal += subtotal;
                    totalTotal += total;

                    // Crear DTO para respuesta
                    var detalleDTO = new DetalleOrdenDTO
                    {
                        DetalleOrdenId = 0, // Se asignará después de guardar
                        ProductoId = producto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        Impuesto = impuesto,
                        Subtotal = subtotal,
                        Total = total
                    };
                    detallesDTO.Add(detalleDTO);
                }

                // Si hay errores, cancelar todo
                if (errors.Count > 0)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<OrdenDTO>
                    {
                        Success = false,
                        Message = "Error al procesar la orden",
                        Errors = errors,
                        Data = null
                    };
                }

                // Agregar todos los detalles
                _context.DetalleOrdenes.AddRange(detalles);

                //  Actualizar orden con totales
                orden.Impuesto = impuestoTotal;
                orden.Subtotal = subtotalTotal;
                orden.Total = totalTotal;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Asignar IDs a los DTOs
                for (int i = 0; i < detalles.Count; i++)
                {
                    detallesDTO[i].DetalleOrdenId = detalles[i].DetalleOrdenId;
                }

                //  Crear respuesta
                var ordenDTO = new OrdenDTO
                {
                    OrdenId = orden.OrdenId,
                    ClienteId = orden.ClienteId,
                    Impuesto = orden.Impuesto,
                    Subtotal = orden.Subtotal,
                    Total = orden.Total,
                    FechaCreacion = orden.FechaCreacion,
                    Detalles = detallesDTO
                };

                return new ApiResponse<OrdenDTO>
                {
                    Success = true,
                    Message = "Orden creada exitosamente",
                    Errors = new List<string>(),
                    Data = ordenDTO
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " | Inner: " + ex.InnerException.Message;

                return new ApiResponse<OrdenDTO>
                {
                    Success = false,
                    Message = "Error inesperado al crear la orden",
                    Errors = new List<string> { errorMessage },
                    Data = null
                };
            }
        }
    }
}