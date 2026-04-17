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

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            using var connection = ObtenerConexion();

            var clientes = await connection.QueryAsync<Usuario>(
                "spClienteListar",
                commandType: CommandType.StoredProcedure
            );

            return View(clientes);
        }

        // GET: Cliente/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Cliente/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario cliente)
        {
            // Paradigma procedural:
            // Se valida el modelo, luego se ejecuta el procedimiento y finalmente se redirige.
            if (!ModelState.IsValid)
                return View(cliente);

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

            TempData["Mensaje"] = "Cliente registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Cliente/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            using var connection = ObtenerConexion();

            var cliente = await connection.QueryFirstOrDefaultAsync<Usuario>(
                "spClienteObtenerPorId",
                new { idUsuario = id },
                commandType: CommandType.StoredProcedure
            );

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Cliente/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Usuario cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

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

            TempData["Mensaje"] = "Cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Cliente/Eliminar/5
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
    }
}