using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public bool Estado { get; set; }

        public int? IdCategoria { get; set; }
        public int? IdProveedor { get; set; }

        public Categoria? Categoria { get; set; }
        public Proveedor? Proveedor { get; set; }
    }
}