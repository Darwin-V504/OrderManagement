namespace Prueba_Completa.DTOs
{
    public class OrdenDTO
    {
        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DetalleOrdenDTO> Detalles { get; set; } = new List<DetalleOrdenDTO>();
    }

    public class DetalleOrdenDTO
    {
        public long DetalleOrdenId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    public class OrdenCreateDTO
    {
        public long ClienteId { get; set; }
        public List<DetalleCreateDTO> Detalles { get; set; } = new List<DetalleCreateDTO>();
    }

    public class DetalleCreateDTO
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}