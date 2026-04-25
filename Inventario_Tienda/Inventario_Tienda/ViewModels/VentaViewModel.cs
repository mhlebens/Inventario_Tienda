using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.ViewModels
{
    public class VentaViewModel
    {
        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Display(Name = "Empleado")]
        public int? IdEmpleado { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<DetalleVentaViewModel> Detalles { get; set; } = new();

        public decimal Total { get; set; }
    }
}