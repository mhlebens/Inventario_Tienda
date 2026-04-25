namespace Inventario_Tienda.ViewModels
{
    public class VentaHistorialViewModel
    {
        public int IdCompra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public string Cliente { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
    }
}