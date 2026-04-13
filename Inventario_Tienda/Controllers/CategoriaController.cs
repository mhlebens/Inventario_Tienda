using Microsoft.AspNetCore.Mvc;
using Inventario_Tienda.Models;
using Inventario_Tienda.Interfaces;

namespace Inventario_Tienda.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _repository.GetAllAsync();
            return View(categorias);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(categoria);
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)

        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
