using Dapper;
using Inventario_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IConfiguration _configuration;

        public CategoriaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            using var connection = ObtenerConexion();

            var categorias = await connection.QueryAsync<Categoria>(
                "spCategoriaListar",
                commandType: CommandType.StoredProcedure
            );

            return View(categorias);
        }

        // GET: Categoria/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Categoria/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spCategoriaCrear",
                new
                {
                    nombre = categoria.Nombre
                },
                commandType: CommandType.StoredProcedure
            );

            return RedirectToAction(nameof(Index));
        }

        // GET: Categoria/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            using var connection = ObtenerConexion();

            var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(
                "spCategoriaObtenerPorId",
                new
                {
                    idCategoria = id
                },
                commandType: CommandType.StoredProcedure
            );

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Categoria/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spCategoriaActualizar",
                new
                {
                    idCategoria = categoria.IdCategoria,
                    nombre = categoria.Nombre
                },
                commandType: CommandType.StoredProcedure
            );

            return RedirectToAction(nameof(Index));
        }

        // POST: Categoria/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var connection = ObtenerConexion();

            try
            {
                await connection.ExecuteAsync(
                    "spCategoriaEliminar",
                    new
                    {
                        idCategoria = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                TempData["Mensaje"] = "Categoría eliminada correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] = "No se puede eliminar la categoría porque está relacionada con uno o más productos.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}