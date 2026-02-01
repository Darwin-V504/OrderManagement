namespace Prueba_Completa.DTOs
{
    public class ClienteDTO
    {
        public long ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Identidad { get; set; } = string.Empty;
    }

    public class ClienteCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Identidad { get; set; } = string.Empty;
    }

    public class ClienteUpdateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Identidad { get; set; } = string.Empty;
    }
}