using Dapper;
using Inventario_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClienteController(IConfiguration configuration)
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

            var clientes = await connection.QueryAsync<Usuario>(
                "spClienteListar",
                commandType: CommandType.StoredProcedure
            );

            return View(clientes);
        }

        [HttpGet]
        public IActionResult CrearModal()
        {
            return PartialView("_ClienteModalPartial", new Usuario());
        }

        [HttpGet]
        public async Task<IActionResult> EditarModal(int id)
        {
            using var connection = ObtenerConexion();

            var cliente = await connection.QueryFirstOrDefaultAsync<Usuario>(
                "spClienteObtenerPorId",
                new { idUsuario = id },
                commandType: CommandType.StoredProcedure
            );

            if (cliente == null)
                return NotFound();

            return PartialView("_ClienteModalPartial", cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAjax(Usuario cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spClienteCrear",
                new
                {
                    nombre = cliente.Nombre,
                    telefono = cliente.Telefono,
                    correo = cliente.Correo
                },
                commandType: CommandType.StoredProcedure
            );

            var clientes = await ListarClientes(connection);
            return PartialView("_ClienteTablePartial", clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAjax(Usuario cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spClienteActualizar",
                new
                {
                    idUsuario = cliente.IdUsuario,
                    nombre = cliente.Nombre,
                    telefono = cliente.Telefono,
                    correo = cliente.Correo
                },
                commandType: CommandType.StoredProcedure
            );

            var clientes = await ListarClientes(connection);
            return PartialView("_ClienteTablePartial", clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var connection = ObtenerConexion();

            try
            {
                await connection.ExecuteAsync(
                    "spClienteEliminar",
                    new { idUsuario = id },
                    commandType: CommandType.StoredProcedure
                );

                TempData["Mensaje"] = "Cliente eliminado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] = "No se puede eliminar el cliente porque está relacionado con una venta.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static async Task<IEnumerable<Usuario>> ListarClientes(SqlConnection connection)
        {
            return await connection.QueryAsync<Usuario>(
                "spClienteListar",
                commandType: CommandType.StoredProcedure
            );
        }
    }
}