CREATE DATABASE TiendaDB;
GO

USE TiendaDB;
GO

CREATE TABLE Categoria (
    idCategoria INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Proveedor (
    idProveedor INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    correo VARCHAR(100)
);

CREATE TABLE Producto (
    idProducto INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255),
    precioCompra DECIMAL(10,2) NOT NULL,
    precioVenta DECIMAL(10,2) NOT NULL,
    stockActual INT NOT NULL,
	estado BIT NOT NULL DEFAULT 1,
    idCategoria INT,
    idProveedor INT,

    FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria),
    FOREIGN KEY (idProveedor) REFERENCES Proveedor(idProveedor)
);

CREATE TABLE Usuario (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    rol VARCHAR(50),
    telefono VARCHAR(20),
    correo VARCHAR(100)
);

CREATE TABLE Compra (
    idCompra INT PRIMARY KEY IDENTITY(1,1),
    fecha DATETIME NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    idCliente INT,
    idEmpleado INT,

    FOREIGN KEY (idCliente) REFERENCES Usuario(idUsuario),
    FOREIGN KEY (idEmpleado) REFERENCES Usuario(idUsuario)
);

CREATE TABLE DetalleCompra (
    idDetalleCompra INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    idCompra INT,
    idProducto INT,

    FOREIGN KEY (idCompra) REFERENCES Compra(idCompra),
    FOREIGN KEY (idProducto) REFERENCES Producto(idProducto)
);

/* 15/04/2026 - Procedimientos creados por María Fernanda Mata Halleslebens*/
USE TiendaDB;
GO

/* =========================================================
   STORED PROCEDURES - CATEGORIA
   ========================================================= */

-- Listar categorías
CREATE OR ALTER PROCEDURE spCategoriaListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idCategoria,
        nombre
    FROM Categoria
    ORDER BY nombre;
END;
GO

-- Obtener categoría por ID
CREATE OR ALTER PROCEDURE spCategoriaObtenerPorId
    @idCategoria INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idCategoria,
        nombre
    FROM Categoria
    WHERE idCategoria = @idCategoria;
END;
GO

-- Crear categoría
CREATE OR ALTER PROCEDURE spCategoriaCrear
    @nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Categoria (nombre)
    VALUES (@nombre);
END;
GO

-- Actualizar categoría
CREATE OR ALTER PROCEDURE spCategoriaActualizar
    @idCategoria INT,
    @nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Categoria
    SET nombre = @nombre
    WHERE idCategoria = @idCategoria;
END;
GO

-- Eliminar categoría
CREATE OR ALTER PROCEDURE spCategoriaEliminar
    @idCategoria INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Categoria
    WHERE idCategoria = @idCategoria;
END;
GO


/* =========================================================
   STORED PROCEDURES - PROVEEDOR
   ========================================================= */

-- Listar proveedores
CREATE OR ALTER PROCEDURE spProveedorListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProveedor,
        nombre,
        telefono,
        correo
    FROM Proveedor
    ORDER BY nombre;
END;
GO

-- Obtener proveedor por ID
CREATE OR ALTER PROCEDURE spProveedorObtenerPorId
    @idProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idProveedor,
        nombre,
        telefono,
        correo
    FROM Proveedor
    WHERE idProveedor = @idProveedor;
END;
GO

-- Crear proveedor
CREATE OR ALTER PROCEDURE spProveedorCrear
    @nombre NVARCHAR(100),
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Proveedor (nombre, telefono, correo)
    VALUES (@nombre, @telefono, @correo);
END;
GO

-- Actualizar proveedor
CREATE OR ALTER PROCEDURE spProveedorActualizar
    @idProveedor INT,
    @nombre NVARCHAR(100),
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Proveedor
    SET nombre = @nombre,
        telefono = @telefono,
        correo = @correo
    WHERE idProveedor = @idProveedor;
END;
GO

-- Eliminar proveedor
CREATE OR ALTER PROCEDURE spProveedorEliminar
    @idProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Proveedor
    WHERE idProveedor = @idProveedor;
END;
GO


/*Procedimientos de Productos*/
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

USE TiendaDB;
GO

CREATE OR ALTER PROCEDURE spProductoListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.idProducto AS IdProducto,
        p.nombre AS Nombre,
        p.descripcion AS Descripcion,
        p.precioCompra AS PrecioCompra,
        p.precioVenta AS PrecioVenta,
        p.stockActual AS StockActual,
        p.estado AS Estado,
        p.idCategoria AS IdCategoria,
        p.idProveedor AS IdProveedor,
        c.nombre AS NombreCategoria,
        pr.nombre AS NombreProveedor
    FROM Producto p
    LEFT JOIN Categoria c ON p.idCategoria = c.idCategoria
    LEFT JOIN Proveedor pr ON p.idProveedor = pr.idProveedor;
END;
GO

CREATE OR ALTER PROCEDURE spProductoObtenerPorId
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idProducto AS IdProducto,
        nombre AS Nombre,
        descripcion AS Descripcion,
        precioCompra AS PrecioCompra,
        precioVenta AS PrecioVenta,
        stockActual AS StockActual,
        estado AS Estado,
        idCategoria AS IdCategoria,
        idProveedor AS IdProveedor
    FROM Producto
    WHERE idProducto = @idProducto;
END;
GO

--- Procedimientos para el módulo de Ventas (16-04-26)

-- Listar Usuarios
CREATE OR ALTER PROCEDURE spUsuarioListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idUsuario AS IdUsuario,
        nombre AS Nombre,
        rol AS Rol,
        telefono AS Telefono,
        correo AS Correo
    FROM Usuario
    ORDER BY nombre;
END;
GO

-- Listar productos para venta
CREATE OR ALTER PROCEDURE spProductoListarDisponiblesVenta
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idProducto AS IdProducto,
        nombre AS Nombre,
        precioVenta AS PrecioVenta,
        stockActual AS StockActual
    FROM Producto
    WHERE estado = 1 AND stockActual > 0
    ORDER BY nombre;
END;
GO

-- Obtener stock
CREATE OR ALTER PROCEDURE spProductoObtenerStock
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT stockActual
    FROM Producto
    WHERE idProducto = @idProducto;
END;
GO

--Descontar stock
CREATE OR ALTER PROCEDURE spProductoDescontarStock
    @idProducto INT,
    @cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Producto
    SET stockActual = stockActual - @cantidad
    WHERE idProducto = @idProducto;
END;
GO

-- Crear compra y devolver id
CREATE OR ALTER PROCEDURE spCompraCrear
    @fecha DATETIME,
    @total DECIMAL(10,2),
    @idCliente INT = NULL,
    @idEmpleado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Compra (fecha, total, idCliente, idEmpleado)
    VALUES (@fecha, @total, @idCliente, @idEmpleado);

    SELECT SCOPE_IDENTITY();
END;
GO

--Crear Detalle
CREATE OR ALTER PROCEDURE spDetalleCompraCrear
    @cantidad INT,
    @subtotal DECIMAL(10,2),
    @idCompra INT,
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO DetalleCompra (cantidad, subtotal, idCompra, idProducto)
    VALUES (@cantidad, @subtotal, @idCompra, @idProducto);
END;
GO

/*Procedimientos del módulo de ventas*/

USE TiendaDB;
GO

/* ============================
   CLIENTES (tabla Usuario)
   ============================ */

CREATE OR ALTER PROCEDURE spClienteListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idUsuario AS IdUsuario,
        nombre AS Nombre,
        rol AS Rol,
        telefono AS Telefono,
        correo AS Correo
    FROM Usuario
    WHERE rol = 'Cliente'
    ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE spClienteObtenerPorId
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idUsuario AS IdUsuario,
        nombre AS Nombre,
        rol AS Rol,
        telefono AS Telefono,
        correo AS Correo
    FROM Usuario
    WHERE idUsuario = @idUsuario
      AND rol = 'Cliente';
END;
GO

CREATE OR ALTER PROCEDURE spClienteCrear
    @nombre NVARCHAR(100),
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuario (nombre, rol, telefono, correo)
    VALUES (@nombre, 'Cliente', @telefono, @correo);
END;
GO

CREATE OR ALTER PROCEDURE spClienteActualizar
    @idUsuario INT,
    @nombre NVARCHAR(100),
    @telefono NVARCHAR(20) = NULL,
    @correo NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuario
    SET nombre = @nombre,
        telefono = @telefono,
        correo = @correo
    WHERE idUsuario = @idUsuario
      AND rol = 'Cliente';
END;
GO

CREATE OR ALTER PROCEDURE spClienteEliminar
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Usuario
    WHERE idUsuario = @idUsuario
      AND rol = 'Cliente';
END;
GO

CREATE OR ALTER PROCEDURE spClienteListarParaVenta
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idUsuario AS IdUsuario,
        nombre AS Nombre
    FROM Usuario
    WHERE rol = 'Cliente'
    ORDER BY nombre;
END;
GO

CREATE OR ALTER PROCEDURE spEmpleadoListarParaVenta
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        idUsuario AS IdUsuario,
        nombre AS Nombre
    FROM Usuario
    WHERE rol = 'Empleado'
    ORDER BY nombre;
END;
GO

INSERT INTO Usuario (nombre, rol, telefono, correo)
VALUES 
('María Mata', 'Empleado', '8888-1111', 'maria@tienda.com'),
('Daniel Rojas', 'Empleado', '8888-2222', 'daniel@tienda.com');

CREATE OR ALTER PROCEDURE spVentaHistorialListar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.idCompra AS IdCompra,
        c.fecha AS Fecha,
        c.total AS Total,
        ISNULL(cli.nombre, 'Sin cliente') AS Cliente,
        ISNULL(emp.nombre, 'Sin empleado') AS Empleado
    FROM Compra c
    LEFT JOIN Usuario cli ON c.idCliente = cli.idUsuario
    LEFT JOIN Usuario emp ON c.idEmpleado = emp.idUsuario
    ORDER BY c.fecha DESC, c.idCompra DESC;
END;
GO 

