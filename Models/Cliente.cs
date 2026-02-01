using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prueba_Completa.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public long ClienteId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Identidad { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}