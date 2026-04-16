using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
}