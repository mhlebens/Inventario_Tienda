namespace Inventario_Tienda.ViewModels
{
    public class DetalleVentaViewModel
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int StockDisponible { get; set; }

        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}