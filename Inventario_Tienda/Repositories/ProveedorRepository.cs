using System.Data;
using Inventario_Tienda.Interfaces;
using Inventario_Tienda.Models;
using Microsoft.Data.SqlClient;
using Dapper;



namespace Inventario_Tienda.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly string _connectionString;

        public ProveedorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException("La cadena de conexión no se encontró");
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<Proveedor>("SELECT Id, NombreContacto, Telfono FROM Proveedores");
        }

        public async Task AddAsync(Proveedor proveedor)
        {
            using var db = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Proveedores (NombreContaco, Telefono) VALUES (@NombreContacto, @Telefono)";
            await db.ExecuteAsync(sql, proveedor);
        }
    }
}
