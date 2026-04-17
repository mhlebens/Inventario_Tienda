using System.ComponentModel.DataAnnotations;

namespace Inventario_Tienda.Models
{
    /* Paradigma orientado a objetos:
    Esta clase representa la entidad Compra dentro del sistema */
    public class Compra
    {
        [Key]
        public int IdCompra { get; set; }

        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public int? IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
    }
}