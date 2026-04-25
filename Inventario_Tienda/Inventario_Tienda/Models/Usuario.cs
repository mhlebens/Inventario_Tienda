using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    // Paradigma orientado a objetos:
    // Esta clase representa la entidad Usuario dentro del sistema
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
}