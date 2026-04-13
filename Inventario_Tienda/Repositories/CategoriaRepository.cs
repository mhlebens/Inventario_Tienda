using System.Data;
using Inventario_Tienda.Interfaces;
using Inventario_Tienda.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Inventario_Tienda.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                              ?? throw new ArgumentNullException("No se logró encontrar la cadena de conexión");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            using var db = Connection;
            return await db.QueryAsync<Categoria>("SELECT Id, Nombre FROM Categorias");
        }
        public async Task<Categoria> GetIDAsync(int id)
        {
            using var db = Connection;
            return await db.QueryFirstOrDefaultAsync<Categoria>("SELECT Id, Nombre FROM Categorias WHERE Id = @Id", new { Id = id });
        }

        public async Task AddAsync(Categoria categoria)
        {
            using var db = Connection;
            string sql = "INSERT INTO Categorias (Nombre) VALUES (@Nombre)";
            await db.ExecuteAsync(sql, categoria);
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            using var db = Connection;
            string sql = "UPDATE Categorias SET Nombre = @Nombre WHERE Id = @Id";
            await db.ExecuteAsync(sql, categoria);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = Connection;
            string sql = "DELETE FROM Categorias WHERE Id = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
