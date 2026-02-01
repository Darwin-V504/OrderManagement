using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba_Completa.Models
{
    [Table("DetalleOrden")]
    public class DetalleOrden
    {
        public long DetalleOrdenId { get; set; }
        public long OrdenId { get; set; }
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; } = null!;

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;
    }
}