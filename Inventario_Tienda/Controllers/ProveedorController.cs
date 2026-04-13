using Microsoft.AspNetCore.Mvc;
using Inventario_Tienda.Models;
using Inventario_Tienda.Interfaces;

namespace Inventario_Tienda.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorRepository _repository;

        public ProveedorController(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _repository.GetAllAsync();
            return View(proveedores);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(proveedor);
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }
        
    }
}
