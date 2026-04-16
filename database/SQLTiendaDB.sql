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
