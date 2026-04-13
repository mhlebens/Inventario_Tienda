using Inventario_Tienda.Models;

namespace Inventario_Tienda.Interfaces
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task AddAsync(Proveedor proveedor);
    }
}
