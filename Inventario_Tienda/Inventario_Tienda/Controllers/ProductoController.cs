using Inventario_Tienda.Data;
using Inventario_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Inventario_Tienda.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult VerProducto()
        {
            var lista = _context.Producto
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .ToList();

            return View(lista);
        }

        public IActionResult Crear()
        {
            CargarDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                CargarDropdowns();
                return View(producto);
            }

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Producto_Crear @nombre, @descripcion, @precioCompra, @precioVenta, @stockActual, @estado, @idCategoria, @idProveedor",
                new SqlParameter("@nombre", producto.Nombre),
                new SqlParameter("@descripcion", (object?)producto.Descripcion ?? DBNull.Value),
                new SqlParameter("@precioCompra", producto.PrecioCompra),
                new SqlParameter("@precioVenta", producto.PrecioVenta),
                new SqlParameter("@stockActual", producto.StockActual),
                new SqlParameter("@estado", producto.Estado),
                new SqlParameter("@idCategoria", (object?)producto.IdCategoria ?? DBNull.Value),
                new SqlParameter("@idProveedor", (object?)producto.IdProveedor ?? DBNull.Value)
            );

            return RedirectToAction(nameof(VerProducto));
        }

        public IActionResult Editar(int id)
        {
            var producto = _context.Producto.FirstOrDefault(p => p.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            CargarDropdowns();
            return View(producto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                CargarDropdowns();
                return View(producto);
            }

            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Producto_Actualizar @idProducto, @nombre, @descripcion, @precioCompra, @precioVenta, @stockActual, @estado, @idCategoria, @idProveedor",
                new SqlParameter("@idProducto", producto.IdProducto),
                new SqlParameter("@nombre", producto.Nombre),
                new SqlParameter("@descripcion", (object?)producto.Descripcion ?? DBNull.Value),
                new SqlParameter("@precioCompra", producto.PrecioCompra),
                new SqlParameter("@precioVenta", producto.PrecioVenta),
                new SqlParameter("@stockActual", producto.StockActual),
                new SqlParameter("@estado", producto.Estado),
                new SqlParameter("@idCategoria", (object?)producto.IdCategoria ?? DBNull.Value),
                new SqlParameter("@idProveedor", (object?)producto.IdProveedor ?? DBNull.Value)
            );

            return RedirectToAction(nameof(VerProducto));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Producto_Eliminar @idProducto",
                new SqlParameter("@idProducto", id)
            );

            return RedirectToAction(nameof(VerProducto));
        }

        private void CargarDropdowns()
        {
            ViewBag.Categorias = new SelectList(_context.Categoria.ToList(), "IdCategoria", "Nombre");
            ViewBag.Proveedores = new SelectList(_context.Proveedor.ToList(), "IdProveedor", "Nombre");
        }
    }
}