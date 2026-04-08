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

