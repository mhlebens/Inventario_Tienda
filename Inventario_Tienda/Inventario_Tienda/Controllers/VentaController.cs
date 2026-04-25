using Dapper;
using Inventario_Tienda.Models;
using Inventario_Tienda.Services;
using Inventario_Tienda.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Controllers
{
    public class VentaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly VentaService _ventaService;

        public VentaController(IConfiguration configuration, VentaService ventaService)
        {
            _configuration = configuration;
            _ventaService = ventaService;
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        /* Paradigma orientado a objetos:
         El controller trabaja con objetos del dominio y viewmodels
         para transportar y organizar la información de la venta. */
        public async Task<IActionResult> Registrar()
        {
            var model = new VentaViewModel();
            await CargarCombos();
            await CargarProductosDisponibles(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(VentaViewModel model)
        {
            await CargarCombos();

            if (!ModelState.IsValid)
            {
                await CompletarDetallesVenta(model);
                return View(model);
            }

            var resultado = await _ventaService.RegistrarVentaAsync(model);

            if (!resultado.ok)
            {
                // Recargar datos visuales sin perder cantidades ingresadas
                await CompletarDetallesVenta(model);

                TempData["Error"] = resultado.mensaje;
                return View(model);
            }

            TempData["Mensaje"] = resultado.mensaje;
            return RedirectToAction(nameof(Historial));
        }

        public async Task<IActionResult> Historial()
        {
            using var connection = ObtenerConexion();

            var ventas = await connection.QueryAsync<VentaHistorialViewModel>(
                "spVentaHistorialListar",
                commandType: CommandType.StoredProcedure
            );

            return View(ventas);
        }

        private async Task CargarCombos()
        {
            using var connection = ObtenerConexion();

            var clientes = await connection.QueryAsync<Usuario>(
                "spClienteListarParaVenta",
                commandType: CommandType.StoredProcedure
            );

            var empleados = await connection.QueryAsync<Usuario>(
                "spEmpleadoListarParaVenta",
                commandType: CommandType.StoredProcedure
            );

            ViewBag.Clientes = new SelectList(clientes, "IdUsuario", "Nombre");
            ViewBag.Empleados = new SelectList(empleados, "IdUsuario", "Nombre");
        }

        private async Task CargarProductosDisponibles(VentaViewModel model)
        {
            using var connection = ObtenerConexion();

            var productos = await connection.QueryAsync<dynamic>(
                "spProductoListarDisponiblesVenta",
                commandType: CommandType.StoredProcedure
            );

            model.Detalles = productos.Select(p => new DetalleVentaViewModel
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.Nombre,
                PrecioVenta = p.PrecioVenta,
                StockDisponible = p.StockActual,
                Cantidad = 0,
                Subtotal = 0
            }).ToList();
        }

        private async Task CompletarDetallesVenta(VentaViewModel model)
        {
            if (model.Detalles == null || !model.Detalles.Any())
            {
                await CargarProductosDisponibles(model);
                return;
            }

            using var connection = ObtenerConexion();

            var productos = await connection.QueryAsync<dynamic>(
                "spProductoListarDisponiblesVenta",
                commandType: CommandType.StoredProcedure
            );

            foreach (var detalle in model.Detalles)
            {
                var producto = productos.FirstOrDefault(p => p.IdProducto == detalle.IdProducto);

                if (producto != null)
                {
                    detalle.NombreProducto = producto.Nombre;
                    detalle.PrecioVenta = producto.PrecioVenta;
                    detalle.StockDisponible = producto.StockActual;
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioVenta;
                }
            }

            // Paradigma funcional:
            // Se calcula el total sumando únicamente los subtotales válidos.
            model.Total = model.Detalles
                .Where(d => d.Cantidad > 0)
                .Sum(d => d.Subtotal);
        }
    }
}