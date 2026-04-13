USE master
GO

CREATE DATABASE Inventario
GO

USE Inventario
GO

SELECT * FROM sqProductoCrear

CREATE OR ALTER PROCEDURE spProductoCrear
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255) = NULL,
    @precioCompra DECIMAL(10,2),
    @precioVenta DECIMAL(10,2),
    @stockActual INT,
    @estado BIT,
    @idCategoria INT = NULL,
    @idProveedor INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Producto
    (
        nombre,
        descripcion,
        precioCompra,
        precioVenta,
        stockActual,
        estado,
        idCategoria,
        idProveedor
    )
    VALUES
    (
        @nombre,
        @descripcion,
        @precioCompra,
        @precioVenta,
        @stockActual,
        @estado,
        @idCategoria,
        @idProveedor
    );
END;
GO


CREATE OR ALTER PROCEDURE spProductoActualizar
    @idProducto INT,
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255) = NULL,
    @precioCompra DECIMAL(10,2),
    @precioVenta DECIMAL(10,2),
    @stockActual INT,
    @estado BIT,
    @idCategoria INT = NULL,
    @idProveedor INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Producto
    SET
        nombre = @nombre,
        descripcion = @descripcion,
        precioCompra = @precioCompra,
        precioVenta = @precioVenta,
        stockActual = @stockActual,
        estado = @estado,
        idCategoria = @idCategoria,
        idProveedor = @idProveedor
    WHERE idProducto = @idProducto;
END;
GO


CREATE OR ALTER PROCEDURE spProductoEliminar
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Producto
    WHERE idProducto = @idProducto;
END;
GO