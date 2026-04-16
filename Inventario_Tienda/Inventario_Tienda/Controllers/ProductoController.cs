using Dapper;
using Inventario_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // GET: Producto/VerProducto
        public async Task<IActionResult> VerProducto()
        {
            using var connection = ObtenerConexion();

            var productos = await connection.QueryAsync<Producto>(
                "spProductoListar",
                commandType: CommandType.StoredProcedure
            );

            return View(productos.ToList());
        }

        // GET: Producto/Crear
        public async Task<IActionResult> Crear()
        {
            await CargarDropdowns();
            return View();
        }

        // POST: Producto/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                await CargarDropdowns();
                return View(producto);
            }

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spProductoCrear",
                new
                {
                    nombre = producto.Nombre,
                    descripcion = producto.Descripcion,
                    precioCompra = producto.PrecioCompra,
                    precioVenta = producto.PrecioVenta,
                    stockActual = producto.StockActual,
                    estado = producto.Estado,
                    idCategoria = producto.IdCategoria,
                    idProveedor = producto.IdProveedor
                },
                commandType: CommandType.StoredProcedure
            );

            return RedirectToAction(nameof(VerProducto));
        }

        // GET: Producto/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            using var connection = ObtenerConexion();

            var producto = await connection.QueryFirstOrDefaultAsync<Producto>(
                "spProductoObtenerPorId",
                new
                {
                    idProducto = id
                },
                commandType: CommandType.StoredProcedure
            );

            if (producto == null)
                return NotFound();

            await CargarDropdowns();
            return View(producto);
        }

        // POST: Producto/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                await CargarDropdowns();
                return View(producto);
            }

            using var connection = ObtenerConexion();

            await connection.ExecuteAsync(
                "spProductoActualizar",
                new
                {
                    idProducto = producto.IdProducto,
                    nombre = producto.Nombre,
                    descripcion = producto.Descripcion,
                    precioCompra = producto.PrecioCompra,
                    precioVenta = producto.PrecioVenta,
                    stockActual = producto.StockActual,
                    estado = producto.Estado,
                    idCategoria = producto.IdCategoria,
                    idProveedor = producto.IdProveedor
                },
                commandType: CommandType.StoredProcedure
            );

            return RedirectToAction(nameof(VerProducto));
        }

        // POST: Producto/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            using var connection = ObtenerConexion();

            try
            {
                await connection.ExecuteAsync(
                    "spProductoEliminar",
                    new
                    {
                        idProducto = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }
            catch (SqlException)
            {
                TempData["Error"] = "No se puede eliminar el producto porque está relacionado con otros registros.";
            }

            return RedirectToAction(nameof(VerProducto));
        }

        private async Task CargarDropdowns()
        {
            using var connection = ObtenerConexion();

            var categorias = await connection.QueryAsync<Categoria>(
                "spCategoriaListar",
                commandType: CommandType.StoredProcedure
            );

            var proveedores = await connection.QueryAsync<Proveedor>(
                "spProveedorListar",
                commandType: CommandType.StoredProcedure
            );

            ViewBag.Categorias = new SelectList(categorias, "IdCategoria", "Nombre");
            ViewBag.Proveedores = new SelectList(proveedores, "IdProveedor", "Nombre");
        }
    }
}