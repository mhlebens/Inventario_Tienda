using Dapper;
using Inventario_Tienda.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Inventario_Tienda.Services
{
    public class VentaService
    {
        private readonly IConfiguration _configuration;

        public VentaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        /* Paradigma procedural (Programación Imperativa):
           La venta sigue una secuencia de pasos:
           1. validar datos
           2. validar stock
           3. registrar compra
           4. registrar detalle
           5. descontar inventario */
        public async Task<(bool ok, string mensaje)> RegistrarVentaAsync(VentaViewModel venta)
        {
            using var connection = ObtenerConexion();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                if (!venta.IdCliente.HasValue)
                    return (false, "Debes seleccionar un cliente.");

                if (!venta.IdEmpleado.HasValue)
                    return (false, "Debes seleccionar un empleado.");

                // Paradigma funcional:
                // Se filtran solo los productos con cantidad válida.
                var detallesValidos = (venta.Detalles ?? new List<DetalleVentaViewModel>())
                    .Where(d => d.Cantidad > 0)
                    .ToList();

                if (!detallesValidos.Any())
                    return (false, "Debes agregar al menos un producto con cantidad mayor a 0.");

                // Paradigma funcional:
                // Se recalcula cada subtotal a partir de precio * cantidad.
                detallesValidos.ForEach(d => d.Subtotal = d.PrecioVenta * d.Cantidad);

                // Paradigma funcional:
                // Sum() permite calcular el total general a partir de los subtotales.
                venta.Total = detallesValidos.Sum(d => d.Subtotal);

                // Paradigma estructurado:
                // Se recorre cada detalle para validar stock antes de registrar la venta.
                foreach (var detalle in detallesValidos)
                {
                    var stockActual = await connection.ExecuteScalarAsync<int>(
                        "spProductoObtenerStock",
                        new { idProducto = detalle.IdProducto },
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure
                    );

                    if (detalle.Cantidad > stockActual)
                    {
                        transaction.Rollback();
                        return (false, $"Stock insuficiente para el producto '{detalle.NombreProducto}'.");
                    }
                }

                var idCompraDecimal = await connection.ExecuteScalarAsync<decimal>(
                    "spCompraCrear",
                    new
                    {
                        fecha = venta.Fecha,
                        total = venta.Total,
                        idCliente = venta.IdCliente,
                        idEmpleado = venta.IdEmpleado
                    },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure
                );

                var idCompra = Convert.ToInt32(idCompraDecimal);

                foreach (var detalle in detallesValidos)
                {
                    await connection.ExecuteAsync(
                        "spDetalleCompraCrear",
                        new
                        {
                            cantidad = detalle.Cantidad,
                            subtotal = detalle.Subtotal,
                            idCompra = idCompra,
                            idProducto = detalle.IdProducto
                        },
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure
                    );

                    await connection.ExecuteAsync(
                        "spProductoDescontarStock",
                        new
                        {
                            idProducto = detalle.IdProducto,
                            cantidad = detalle.Cantidad
                        },
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure
                    );
                }

                transaction.Commit();
                return (true, "Venta registrada correctamente.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Ocurrió un error al registrar la venta: {ex.Message}");
            }
        }
    }
}