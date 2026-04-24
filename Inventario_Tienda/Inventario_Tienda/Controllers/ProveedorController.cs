using Dapper;
using Inventario_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProveedorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IActionResult> Index()
        {
            using var connection = ObtenerConexion();

            var proveedores = await connection.QueryAsync<Proveedor>(
                "spProveedorListar",
                commandType: CommandType.StoredProcedure
            );

            return View(proveedores);
        }

        [HttpGet]
        public IActionResult CrearModal()
        {
            return PartialView("_ProveedorModalPartial", new Proveedor());
        }

        [HttpGet]
        public async Task<IActionResult> EditarModal(int id)
        {
            using var connection = ObtenerConexion();

            var proveedor = await connection.QueryFirstOrDefaultAsync<Proveedor>(
                "spProveedorObtenerPorId",
                new { idProveedor = id },
                commandType: CommandType.StoredProcedure
            );

            if (proveedor == null)
                return NotFound();

            return PartialView("_ProveedorModalPartial", proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAjax(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spProveedorCrear",
                new
                {
                    nombre = proveedor.Nombre,
                    telefono = proveedor.Telefono,
                    correo = proveedor.Correo
                },
                commandType: CommandType.StoredProcedure
            );

            var proveedores = await ListarProveedores(connection);
            return PartialView("_ProveedorTablePartial", proveedores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAjax(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spProveedorActualizar",
                new
                {
                    idProveedor = proveedor.IdProveedor,
                    nombre = proveedor.Nombre,
                    telefono = proveedor.Telefono,
                    correo = proveedor.Correo
                },
                commandType: CommandType.StoredProcedure
            );

            var proveedores = await ListarProveedores(connection);
            return PartialView("_ProveedorTablePartial", proveedores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var connection = ObtenerConexion();

            try
            {
                await connection.ExecuteAsync(
                    "spProveedorEliminar",
                    new { idProveedor = id },
                    commandType: CommandType.StoredProcedure
                );

                TempData["Mensaje"] = "Proveedor eliminado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] = "No se puede eliminar el proveedor porque está relacionado con productos.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static async Task<IEnumerable<Proveedor>> ListarProveedores(SqlConnection connection)
        {
            return await connection.QueryAsync<Proveedor>(
                "spProveedorListar",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}