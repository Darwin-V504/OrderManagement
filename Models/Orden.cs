using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba_Completa.Models
{
    [Table("Orden")]
    public class Orden
    {
        public long OrdenId { get; set; }
        public long ClienteId { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;
        public ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();
    }
}