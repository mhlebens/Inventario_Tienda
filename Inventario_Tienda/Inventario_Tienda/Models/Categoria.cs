using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}