using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}