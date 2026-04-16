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

        // GET: Proveedor
        public async Task<IActionResult> Index()
        {
            using var connection = ObtenerConexion();

            var proveedores = await connection.QueryAsync<Proveedor>(
                "spProveedorListar",
                commandType: CommandType.StoredProcedure
            );

            return View(proveedores);
        }

        // GET: Proveedor/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Proveedor/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

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

            return RedirectToAction(nameof(Index));
        }

        // GET: Proveedor/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            using var connection = ObtenerConexion();

            var proveedor = await connection.QueryFirstOrDefaultAsync<Proveedor>(
                "spProveedorObtenerPorId",
                new
                {
                    idProveedor = id
                },
                commandType: CommandType.StoredProcedure
            );

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        // POST: Proveedor/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

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

            return RedirectToAction(nameof(Index));
        }

        // POST: Proveedor/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var connection = ObtenerConexion();

            try
            {
                await connection.ExecuteAsync(
                    "spProveedorEliminar",
                    new
                    {
                        idProveedor = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                TempData["Mensaje"] = "Proveedor eliminado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] = "No se puede eliminar el proveedor porque está relacionado con uno o más productos.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}