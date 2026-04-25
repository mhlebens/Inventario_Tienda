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

        [HttpGet]
        public async Task<IActionResult> CrearModal()
        {
            await CargarCombosProducto();
            return PartialView("_ProductoModalPartial", new Producto());
        }

        [HttpGet]
        public async Task<IActionResult> EditarModal(int id)
        {
            using var connection = ObtenerConexion();

            var producto = await connection.QueryFirstOrDefaultAsync<Producto>(
                "spProductoObtenerPorId",
                new { idProducto = id },
                commandType: CommandType.StoredProcedure
            );

            if (producto == null)
                return NotFound();

            await CargarCombosProducto();
            return PartialView("_ProductoModalPartial", producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAjax(Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

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
                    idCategoria = producto.IdCategoria,
                    idProveedor = producto.IdProveedor,
                    estado = producto.Estado
                },
                commandType: CommandType.StoredProcedure
            );

            var productos = await connection.QueryAsync<Producto>(
                "spProductoListar",
                commandType: CommandType.StoredProcedure
            );

            return PartialView("_ProductoTablePartial", productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAjax(Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

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
                    idCategoria = producto.IdCategoria,
                    idProveedor = producto.IdProveedor,
                    estado = producto.Estado
                },
                commandType: CommandType.StoredProcedure
            );

            var productos = await connection.QueryAsync<Producto>(
                "spProductoListar",
                commandType: CommandType.StoredProcedure
            );

            return PartialView("_ProductoTablePartial", productos);
        }

        private async Task CargarCombosProducto()
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

            ViewBag.Categorias = categorias.Select(c => new SelectListItem
            {
                Value = c.IdCategoria.ToString(),
                Text = c.Nombre
            }).ToList();

            ViewBag.Proveedores = proveedores.Select(p => new SelectListItem
            {
                Value = p.IdProveedor.ToString(),
                Text = p.Nombre
            }).ToList();
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