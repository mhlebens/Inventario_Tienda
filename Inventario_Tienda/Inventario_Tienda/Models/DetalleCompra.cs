using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    public class DetalleCompra
    {
        [Key]
        public int IdDetalleCompra { get; set; }

        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
    }
}